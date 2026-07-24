#pragma once

#include <stdbool.h>
#include <stdint.h>

#define REG_WHO_AM_I 0x0F
#define IMU_WHO_AM_I_VALUE 0x6C

#define REG_CTRL1_XL 0x10
#define REG_CTRL2_G 0x11
#define REG_CTRL3_C 0x12
#define REG_STATUS_REG 0x1E
#define REG_OUT_TEMP_L 0x20

#define ACCEL_SENSITIVITY_MG_PER_LSB 0.244f
#define GYRO_SENSITIVITY_MDPS_PER_LSB 70
#define TEMP_SENSITIVITY_LSB_PER_C 256
#define TEMP_OFFSET_C 25

typedef struct {
    float accel_g[3];
    float gyro_dps[3];
    float temp_c;
} imu_sample_t;

void imu_spi_init();
bool imu_get_sample(imu_sample_t *out);
float imu_roll_deg(const imu_sample_t *imu);
float imu_pitch_deg(const imu_sample_t *imu);