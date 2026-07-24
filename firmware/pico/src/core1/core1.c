#include "camera_uart.h"
#include "imu_spi.h"
#include "shared_state.h"
#include <math.h>
#include <stdio.h>

#define WAIT_PERIOD_US 1000 * 500
#define FILTER_ALPHA 0.98f

static float angle_deg[3] = {0};

static float complementary_filter(float alpha, float current_angle,
                                  float gyro_angle, float acc_angle) {
    return alpha * (current_angle + gyro_angle) + (1 - alpha) * acc_angle;
}

void core1_mainloop() {
    imu_spi_init();
    camera_uart_init();

    imu_sample_t imu = {0};
    camera_sample_t cam = {0};

    float roll = 0;
    float pitch = 0;

    const float dt = 1 / WAIT_PERIOD_US;
    absolute_time_t next_sample = get_absolute_time();
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

            printf("Raw IMU Sample: acc: %.4f, %.4f, %.4f | gyro: "
                   "%.4f, "
                   "%.4f, %.4f | temp: %.4f\n",
                   imu.accel_g[0], imu.accel_g[1], imu.accel_g[2],
                   imu.gyro_dps[0], imu.gyro_dps[1], imu.gyro_dps[2],
                   imu.temp_c);

            roll = complementary_filter(FILTER_ALPHA, roll, angle_deg[0],
                                        imu_roll_deg(&imu));
            pitch = complementary_filter(FILTER_ALPHA, pitch, angle_deg[1],
                                         imu_pitch_deg(&imu));

            printf("Roll: %.4f (gyro), %.4f (acc), %.4f (filtered)",
                   angle_deg[0], imu_roll_deg(&imu), roll);
            printf("Pitch: %.4f (gyro), %.4f (acc), %.4f (filtered)",
                   angle_deg[1], imu_pitch_deg(&imu), pitch);
        }

        next_sample = delayed_by_us(next_sample, WAIT_PERIOD_US);
        busy_wait_until(next_sample);
    }
}