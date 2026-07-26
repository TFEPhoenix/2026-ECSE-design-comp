#include "core1.h"
#include "shared_state.h"
#include <stdio.h>

#include "camera_uart.h"
#include "gpio_control.h"
#include "hid.h"
#include "imu_spi.h"
#include "pico/multicore.h"
#include "pico/stdlib.h"
#include "shared_state.h"
#include <stdio.h>

int main() {
    // stdio_init_all();

    io_init();
    hid_init();
    shared_state_init();

    multicore_launch_core1(core1_mainloop);

    absolute_time_t next_sample = get_absolute_time();

    while (true) {
        shared_state_t state = shared_state_read();
        // printf("Core 0 Sample: Sequence #: %i, Trigger Pressed: %i, x: %i, y:
        // "
        //        "%i\n\n",
        //        state.seq_number, state.trigger_pressed, state.x, state.y);

        tud_task();
        hid_update(state.x, state.y, state.trigger_pressed);

        next_sample = delayed_by_us(next_sample, 1000);

        busy_wait_until(next_sample);
    }
}
