#include "camera_uart.h"
#include "imu_spi.h"
#include "one_euro_filter.h"
#include "orientation_filter.h"
#include "pico/stdlib.h"
#include "shared_state.h"
#include <math.h>
#include <stdio.h>

#define WAIT_PERIOD_US 1000
#define ORIENTATION_ALPHA 0.98f

#define DEG_TO_RAD (float)(M_PI / 180.0)

// static float W_real = 0.60f;
// static float H_real = 0.34f;
static float W_real = 0.50f;
static float H_real = 0.30f;

#define PRINT_EVERY_N 200

void core1_mainloop() {
    camera_uart_init();
    imu_spi_init();

    orientation_filter_t orient;
    orientation_filter_init(&orient, ORIENTATION_ALPHA);

    one_euro_filter_t filter_u, filter_v;

    one_euro_init(&filter_u, 1.0f, 0.007f, 1.0f);
    one_euro_init(&filter_v, 1.0f, 0.007f, 1.0f);

    const float dt = WAIT_PERIOD_US / 1e6f;

    float last_cam_u = 0.5f;
    float last_cam_v = 0.5f;
    float D_current = 1.0f;

    uint32_t loop_count = 0;
    absolute_time_t next_sample = get_absolute_time();

    while (true) {
        next_sample = delayed_by_us(next_sample, WAIT_PERIOD_US);
        loop_count++;

        camera_sample_t cam = {0};
        if (camera_uart_get_sample(&cam) && cam.found) {
            last_cam_u = 1 - cam.col;
            last_cam_v = cam.row;
            D_current = cam.dist_m;

            orientation_filter_reset(
                &orient); // camera gives us the truth we can reset imu shit

            if (loop_count % PRINT_EVERY_N == 0) {
                // printf("CAM correction: u=%.4f v=%.4f D=%.3fm\n", last_cam_u,
                //        last_cam_v, D_current);
            }
        }

        imu_sample_t imu = {0};
        if (imu_get_sample(&imu)) {
            orientation_filter_update(&orient, &imu, dt);

            float yaw_rad = orient.yaw_deg * DEG_TO_RAD;
            float pitch_rad = orient.pitch_deg * DEG_TO_RAD;

            float du = -(D_current * yaw_rad) / W_real;
            float dv = -(D_current * pitch_rad) / H_real;

            float predicted_u = last_cam_u + du;
            float predicted_v = last_cam_v + dv;

            float smooth_u = one_euro_filter(&filter_u, predicted_u, dt);
            float smooth_v = one_euro_filter(&filter_v, predicted_v, dt);

            // clamp to the screen edges
            if (smooth_u < 0.0f)
                smooth_u = 0.0f;
            if (smooth_u > 1.0f)
                smooth_u = 1.0f;
            if (smooth_v < 0.0f)
                smooth_v = 0.0f;
            if (smooth_v > 1.0f)
                smooth_v = 1.0f;

            uint16_t x = (uint16_t)(smooth_u * 32767.0f);
            uint16_t y = (uint16_t)(smooth_v * 32767.0f);

            shared_state_update_coords(x, y);

            if (loop_count % PRINT_EVERY_N == 0) {
                // printf("cursor: u=%.4f v=%.4f -> x=%u y=%u\n", smooth_u,
                //        smooth_v, x, y);
            }
        }

        busy_wait_until(next_sample);
    }
}