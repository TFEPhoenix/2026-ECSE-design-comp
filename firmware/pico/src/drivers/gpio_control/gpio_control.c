#include "hardware/gpio.h"
#include "pico/stdlib.h"
#include "pico/time.h"

#define ON_TIME_MS 50
#define OFF_TIME_MS 20

#define SWITCH_PIN 11
#define SOLENOID_PIN 10

static volatile bool firing = false;
static bool solenoid_on = false;

static void solenoid_deactivate() {
    solenoid_on = false;
    gpio_put(SOLENOID_PIN, 0);
}

static void solenoid_activate() {
    solenoid_on = true;
    gpio_put(SOLENOID_PIN, 1);
}

static int64_t solenoid_alarm(alarm_id_t id, void *user_data) {
    if (!gpio_get(SWITCH_PIN)) {
        firing = false;
        solenoid_deactivate();
        return 0;
    }
    if (solenoid_on) {
        solenoid_deactivate();
        return OFF_TIME_MS * 1000;
    } else {
        solenoid_activate();
        return ON_TIME_MS * 1000;
    }
}

static void switch_gpio_callback(uint gpio, uint32_t events) {
    if (gpio != SWITCH_PIN)
        return;

    if ((events & GPIO_IRQ_EDGE_RISE) && !firing) {
        firing = true;
        solenoid_activate();
        add_alarm_in_ms(ON_TIME_MS, solenoid_alarm, NULL, false);
    }

    if (events & GPIO_IRQ_EDGE_FALL) {
        firing = false;
        solenoid_deactivate();
    }
}

void io_init() {
    gpio_init(SOLENOID_PIN);
    gpio_set_dir(SOLENOID_PIN, GPIO_OUT);
    gpio_put(SOLENOID_PIN, 0);

    gpio_init(SWITCH_PIN);
    gpio_set_dir(SWITCH_PIN, GPIO_IN);
    gpio_set_irq_enabled_with_callback(SWITCH_PIN,
                                       GPIO_IRQ_EDGE_FALL | GPIO_IRQ_EDGE_RISE,
                                       true, switch_gpio_callback);
}
