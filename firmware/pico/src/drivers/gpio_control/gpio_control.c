#include "hardware/gpio.h"
#include "pico/stdlib.h"
#include "pico/time.h"
#include "shared_state.h"

#define ON_TIME_MS 50
#define OFF_TIME_MS 20
#define DEBOUNCE_MS 15

#define SWITCH_PIN 11
#define SOLENOID_PIN 10

typedef enum { GUN_IDLE, GUN_FIRING_ON, GUN_FIRING_OFF } gun_state_t;

static repeating_timer_t g_timer;
static gun_state_t g_state = GUN_IDLE;
static uint32_t g_state_time_ms = 0;
static uint32_t g_press_time_ms = 0;

static bool timer_callback(repeating_timer_t *rt) {
    bool raw_pressed = gpio_get(SWITCH_PIN);

    if (raw_pressed) {
        if (g_press_time_ms < 100)
            g_press_time_ms += 5;
    } else {
        g_press_time_ms = 0;
    }

    bool debounced_pressed = (g_press_time_ms >= DEBOUNCE_MS);

    switch (g_state) {
    case GUN_IDLE:
        if (debounced_pressed) {
            gpio_put(SOLENOID_PIN, 1);
            shared_state_update_trigger(true);
            g_state = GUN_FIRING_ON;
            g_state_time_ms = 0;
        }
        break;

    case GUN_FIRING_ON:
        g_state_time_ms += 5;
        if (g_state_time_ms >= ON_TIME_MS) {
            gpio_put(SOLENOID_PIN, 0);
            shared_state_update_trigger(false);
            g_state = GUN_FIRING_OFF;
            g_state_time_ms = 0;
        }
        break;

    case GUN_FIRING_OFF:
        g_state_time_ms += 5;
        if (g_state_time_ms >= OFF_TIME_MS) {
            if (debounced_pressed) {
                gpio_put(SOLENOID_PIN, 1);
                shared_state_update_trigger(true);
                g_state = GUN_FIRING_ON;
                g_state_time_ms = 0;
            } else {
                g_state = GUN_IDLE;
            }
        }
        break;
    }

    return true;
}

void io_init() {
    gpio_init(SOLENOID_PIN);
    gpio_set_dir(SOLENOID_PIN, GPIO_OUT);
    gpio_put(SOLENOID_PIN, 0);

    gpio_init(SWITCH_PIN);
    gpio_set_dir(SWITCH_PIN, GPIO_IN);
    gpio_pull_down(SWITCH_PIN);

    shared_state_update_trigger(false);

    add_repeating_timer_ms(-5, timer_callback, NULL, &g_timer);
}