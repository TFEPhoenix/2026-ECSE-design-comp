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

static float W_real = 0.50f;
static float H_real = 0.30f;

void core1_mainloop() {
    camera_uart_init();
    imu_spi_init();

    orientation_filter_t orient;
    orientation_filter_init(&orient, ORIENTATION_ALPHA);

    one_euro_filter_t filter_u, filter_v;
    
    one_euro_init(&filter_u, 1.5f, 0.02f, 1.0f);
    one_euro_init(&filter_v, 1.5f, 0.02f, 1.0f);

    float cursor_u = 0.5f;
    float cursor_v = 0.5f;
    float D_current = 1.0f;
    bool cam_was_valid = false;

    absolute_time_t next_sample = get_absolute_time();
    absolute_time_t last_imu_time = get_absolute_time();

    while (true) {
        next_sample = delayed_by_us(next_sample, WAIT_PERIOD_US);

        imu_sample_t imu = {0};
        bool imu_ready = imu_get_sample(&imu);

        float gy = imu.gyro_dps[1];
        float gz = imu.gyro_dps[2];

        if (fabsf(gy) < 0.5f) gy = 0.0f;
        if (fabsf(gz) < 0.5f) gz = 0.0f;

        float motion_speed = sqrtf(gy * gy + gz * gz);

        camera_sample_t cam = {0};
        bool cam_found = camera_uart_get_sample(&cam) && cam.found;

        if (cam_found) {
            float target_u = 1.0f - cam.col;
            float target_v = cam.row;
            D_current = cam.dist_m;

            if (!cam_was_valid) {
                cursor_u = target_u;
                cursor_v = target_v;
                cam_was_valid = true;
            } else if (motion_speed < 60.0f) {
                float diff_u = target_u - cursor_u;
                float diff_v = target_v - cursor_v;
                float err_dist = sqrtf(diff_u * diff_u + diff_v * diff_v);

                if (err_dist < 0.20f) {
                    const float MAX_STEP = 0.002f; 
                    if (diff_u > MAX_STEP) diff_u = MAX_STEP;
                    if (diff_u < -MAX_STEP) diff_u = -MAX_STEP;
                    if (diff_v > MAX_STEP) diff_v = MAX_STEP;
                    if (diff_v < -MAX_STEP) diff_v = -MAX_STEP;

                    cursor_u += diff_u * 0.30f;
                    cursor_v += diff_v * 0.30f;
                }
            }
        } else {
            cam_was_valid = false;
        }

        if (imu_ready) {
            absolute_time_t now = get_absolute_time();
            float actual_dt = absolute_time_diff_us(last_imu_time, now) / 1e6f;
            last_imu_time = now;

            orientation_filter_update(&orient, &imu, actual_dt);

            float roll_rad = orient.roll_deg * DEG_TO_RAD;
            float cos_r = cosf(roll_rad);
            float sin_r = sinf(roll_rad);

            float pitch_rate = gy * cos_r - gz * sin_r; 
            float yaw_rate   = gy * sin_r + gz * cos_r; 

            float u_vel = -(D_current * (yaw_rate * DEG_TO_RAD)) / W_real;
            float v_vel = -(D_current * (pitch_rate * DEG_TO_RAD)) / H_real;

            cursor_u += u_vel * actual_dt;
            cursor_v += v_vel * actual_dt;

            float smooth_u = one_euro_filter(&filter_u, cursor_u, actual_dt);
            float smooth_v = one_euro_filter(&filter_v, cursor_v, actual_dt);

            if (smooth_u < 0.0f) smooth_u = 0.0f;
            if (smooth_u > 1.0f) smooth_u = 1.0f;
            if (smooth_v < 0.0f) smooth_v = 0.0f;
            if (smooth_v > 1.0f) smooth_v = 1.0f;

            uint16_t x = (uint16_t)(smooth_u * 32767.0f);
            uint16_t y = (uint16_t)(smooth_v * 32767.0f);

            shared_state_update_coords(x, y);
        }

        busy_wait_until(next_sample);
    }
}