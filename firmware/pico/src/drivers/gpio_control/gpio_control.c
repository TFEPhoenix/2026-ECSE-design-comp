#include "hardware/gpio.h"
#include "pico/stdlib.h"
#include "stdio.h"

#define SWITCH_PIN 11
#define SOLENOID_PIN 10

static volatile bool switch_pressed = false;

void switch_gpio_callback(uint gpio, uint32_t events) {
    if (gpio != SWITCH_PIN)
        return;
    if (events & GPIO_IRQ_EDGE_RISE)
        switch_pressed = true;
    else if (events & GPIO_IRQ_EDGE_FALL)
        switch_pressed = false;
}

void io_init() {
    gpio_init(SOLENOID_PIN);
    gpio_set_dir(SOLENOID_PIN, GPIO_OUT);

    gpio_init(SWITCH_PIN);
    gpio_set_dir(SWITCH_PIN, GPIO_IN);
    gpio_set_irq_enabled_with_callback(
        SWITCH_PIN, GPIO_IRQ_EDGE_RISE | GPIO_IRQ_EDGE_FALL, true, &switch_gpio_callback);
}

void gpio_loop() {
    io_init();

    while (1) {
        if (switch_pressed) {
            gpio_put(SOLENOID_PIN, 1);
            sleep_ms(50);
            gpio_put(SOLENOID_PIN, 0);
            sleep_ms(20);
        }
    }
}
