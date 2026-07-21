#include "shared_state.h"
#include "uart_camera.h"
#include <stdio.h>

#define WAIT_PERIOD_US 1000 * 5000

void core1_mainloop() {
    uart_camera_init();

    absolute_time_t next_sample = get_absolute_time();
    while (true) {
        next_sample = delayed_by_us(next_sample, WAIT_PERIOD_US);

        camera_sample_t cam;
        bool have_cam = uart_camera_get_sample(&cam);
        printf("%f, %f", cam.col, cam.row);

        busy_wait_until(next_sample);
    }
}