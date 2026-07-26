#include "core1.h"
#include "shared_state.h"
#include <stdio.h>

#include "gpio_control.h"
#include "imu_spi.h"
#include "hid.h"
#include "pico/multicore.h"
#include "pico/stdlib.h"
#include "shared_state.h"
#include <stdio.h>

int main() {
    stdio_init_all();
    shared_state_init();
    io_init();
    hid_init(); 

    sleep_ms(5000);

    multicore_launch_core1(core1_mainloop);

    absolute_time_t next_sample = get_absolute_time();

    while (true) {
        // next_sample = delayed_by_us(next_sample, 1000 * 2500);

        // shared_state_t state = shared_state_read();
        // printf("\n--CORE 0 SAMPLE:--\n");
        // printf("Data: %i %i %i %i\n\n", state.seq_number, state.trigger_pressed,
        //        state.x, state.y);

        // busy_wait_until(next_sample);

        tud_task();
        hid_test();
    }
}
