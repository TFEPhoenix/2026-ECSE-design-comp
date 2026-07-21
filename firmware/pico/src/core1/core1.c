#include "shared_state.h"
#include <stdio.h>
#define WAIT_PERIOD_US 1000 * 5000

void core1_mainloop() {
    absolute_time_t next_sample = get_absolute_time();

    while (true) {
        next_sample = delayed_by_us(next_sample, WAIT_PERIOD_US);

        printf("hello\n");
        busy_wait_until(next_sample);
    }
}