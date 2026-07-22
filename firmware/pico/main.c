#include "core1.h"
#include "shared_state.h"
#include <stdio.h>

#include "gpio_control.h"
#include "imu_spi.h"
#include "pico/multicore.h"
#include "pico/stdlib.h"
#include <stdio.h>

int main() {
    stdio_init_all();
    shared_state_init();
    io_init();

    sleep_ms(5000);

    multicore_launch_core1(core1_mainloop);

    absolute_time_t next_sample = get_absolute_time();

    while (true) {
        next_sample = delayed_by_us(next_sample, 1000 * 2500);

        printf("\n----Core 0 just wanna be saying smth----\n");
        printf("\n----This is where the usb should send that data now----\n");

        busy_wait_until(next_sample);
    }
}
