#include "camera_uart.h"
#include "filters.h"
#include "imu_spi.h"
#include "one_euro_filter.h"
#include "shared_state.h"
#include <math.h>
#include <stdio.h>

#define WAIT_PERIOD_US 1000
#define FILTER_ALPHA 0.98f

static float angle_deg[3] = {0};

void core1_mainloop() {
    imu_spi_init();
    camera_uart_init();

    imu_sample_t imu = {0};
    camera_sample_t cam = {0};

    float roll = 0;
    float pitch = 0;

    const float dt = WAIT_PERIOD_US / 1e6f;
    absolute_time_t next_sample = get_absolute_time();

    one_euro_filter_t roll_filter;
    one_euro_init(&roll_filter, 1.5, 0.5, 1);

    one_euro_filter_t pitch_filter;
    one_euro_init(&pitch_filter, 1.5, 0.5, 1);

    while (true) {
        bool have_cam = camera_uart_get_sample(&cam);
        if (have_cam) {
            printf("Camera Sample: (%f, %f)\n", cam.col, cam.row);
        }

        bool have_imu = imu_get_sample(&imu);
        if (have_imu) {
            angle_deg[0] += imu.gyro_dps[0] * dt;
            angle_deg[1] += imu.gyro_dps[1] * dt;
            angle_deg[2] += imu.gyro_dps[2] * dt;
            
            roll = complementary_filter(
                FILTER_ALPHA, roll, imu.gyro_dps[0] * dt, imu_roll_deg(&imu));
            pitch = complementary_filter(
                FILTER_ALPHA, pitch, imu.gyro_dps[1] * dt, imu_pitch_deg(&imu));

            // just trying to test out one euro filter, in actual case ill apply at the end of cam+imu
            printf("Roll: %+.4f | ", one_euro_filter(&roll_filter, roll, dt));
            printf("Pitch: %+.4f | ", one_euro_filter(&pitch_filter, pitch, dt));
            printf("Yaw: %+.4f\n", angle_deg[2]);
        }

        next_sample = delayed_by_us(next_sample, WAIT_PERIOD_US);
        busy_wait_until(next_sample);
    }
}