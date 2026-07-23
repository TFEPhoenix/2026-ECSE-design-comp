#include "camera_uart.h"
#include "imu_spi.h"
#include "shared_state.h"
#include <stdio.h>

#define WAIT_PERIOD_US 1000 * 250

void core1_mainloop() {
    camera_uart_init();
    imu_spi_init();

    absolute_time_t next_sample = get_absolute_time();
    while (true) {
        next_sample = delayed_by_us(next_sample, WAIT_PERIOD_US);

        camera_sample_t cam = {0};
        bool have_cam = camera_uart_get_sample(&cam);
        printf("Camera Sample: (%f, %f)\n", cam.col, cam.row);

        imu_sample_t imu = {0};
        bool have_imu = imu_get_sample(&imu);
        printf("IMU Sample: acceleration: %.4f, %.4f, %.4f | gyroscope: %.4f, "
               "%.4f, %.4f | temperature: %.2f\n",
               imu.accel_g[0], imu.accel_g[1], imu.accel_g[2], imu.gyro_dps[0],
               imu.gyro_dps[1], imu.gyro_dps[2], imu.temp_c);

        busy_wait_until(next_sample);
    }
}