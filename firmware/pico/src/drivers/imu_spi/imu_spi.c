#include "imu_spi.h"
#include "hardware/gpio.h"
#include "hardware/spi.h"
#include "pico/stdlib.h"
#include <math.h>
#include <stdio.h>

#define PIN_CS 1
#define PIN_SCK 2
#define PIN_MOSI 3
#define PIN_MISO 4
#define SPI_PORT spi0
// #define CLK_FREQ 500000
#define CLK_FREQ 10000000

#define RAD_TO_DEG (180.0f / M_PI)

static inline void cs_select() { gpio_put(PIN_CS, 0); }
static inline void cs_deselect() { gpio_put(PIN_CS, 1); }

static void calibrate_gyro();
static float gyro_bias[3] = {0.0f, 0.0f, 0.0f};

static void write_reg(uint8_t reg, uint8_t val) {
    uint8_t tx = reg & ~(1 << 7);
    uint8_t packet[] = {tx, val};

    cs_select();
    spi_write_blocking(SPI_PORT, packet, 2);
    cs_deselect();
}

static uint8_t read_reg(uint8_t reg) {
    uint8_t tx = reg | (1 << 7);
    uint8_t rx;

    cs_select();
    spi_write_blocking(SPI_PORT, &tx, 1);
    spi_read_blocking(SPI_PORT, 0x00, &rx, 1);
    cs_deselect();

    return rx;
}

static void burst_read(uint8_t reg, uint8_t *buf, size_t len) {
    uint8_t tx = reg | (1 << 7);

    cs_select();
    spi_write_blocking(SPI_PORT, &tx, 1);
    spi_read_blocking(SPI_PORT, 0x00, buf, len);
    cs_deselect();
}

void imu_spi_init() {
    spi_init(SPI_PORT, CLK_FREQ);
    spi_set_format(SPI_PORT, 8, SPI_CPOL_0, SPI_CPHA_0, SPI_MSB_FIRST);

    gpio_set_function(PIN_MISO, GPIO_FUNC_SPI);
    gpio_set_function(PIN_SCK, GPIO_FUNC_SPI);
    gpio_set_function(PIN_MOSI, GPIO_FUNC_SPI);

    gpio_init(PIN_CS);
    gpio_set_dir(PIN_CS, GPIO_OUT);
    cs_deselect();

    sleep_ms(50);

    uint8_t who = read_reg(REG_WHO_AM_I);
    printf("\nWHOAMI: %x\n", who);

    write_reg(REG_CTRL1_XL, 0x78); // 833 Hz , +/-8g
    write_reg(REG_CTRL2_G, 0x7C);  // 833 Hz, +/-2000dps
    write_reg(REG_CTRL3_C, 0x44);  // bdu, if_inc

    sleep_ms(200);
    calibrate_gyro();
}

static bool imu_data_ready() {
    uint8_t status = read_reg(REG_STATUS_REG);
    return (status & 0x03) != 0;
}

bool imu_get_sample(imu_sample_t *out) {
    if (!imu_data_ready()) {
        return false;
    }

    uint8_t raw[14];
    burst_read(REG_OUT_TEMP_L, raw, sizeof(raw));

    int16_t temp_raw = (int16_t)((raw[1] << 8) | raw[0]);
    int16_t gx_raw = (int16_t)((raw[3] << 8) | raw[2]);
    int16_t gy_raw = (int16_t)((raw[5] << 8) | raw[4]);
    int16_t gz_raw = (int16_t)((raw[7] << 8) | raw[6]);
    int16_t ax_raw = (int16_t)((raw[9] << 8) | raw[8]);
    int16_t ay_raw = (int16_t)((raw[11] << 8) | raw[10]);
    int16_t az_raw = (int16_t)((raw[13] << 8) | raw[12]);

    out->temp_c =
        TEMP_OFFSET_C + ((float)temp_raw / TEMP_SENSITIVITY_LSB_PER_C);

    out->gyro_dps[0] =
        ((float)gx_raw * GYRO_SENSITIVITY_MDPS_PER_LSB) / 1000.0f -
        gyro_bias[0];
    out->gyro_dps[1] =
        ((float)gy_raw * GYRO_SENSITIVITY_MDPS_PER_LSB) / 1000.0f -
        gyro_bias[1];
    out->gyro_dps[2] =
        ((float)gz_raw * GYRO_SENSITIVITY_MDPS_PER_LSB) / 1000.0f -
        gyro_bias[2];

    out->accel_g[0] = ((float)ax_raw * ACCEL_SENSITIVITY_MG_PER_LSB) / 1000.0f;
    out->accel_g[1] = ((float)ay_raw * ACCEL_SENSITIVITY_MG_PER_LSB) / 1000.0f;
    out->accel_g[2] = ((float)az_raw * ACCEL_SENSITIVITY_MG_PER_LSB) / 1000.0f;

    return true;
}

static void calibrate_gyro() {
    const int samples = 500;

    float sum_x = 0.0f;
    float sum_y = 0.0f;
    float sum_z = 0.0f;

    printf("Calibrating Gyro...\n");

    int count = 0;

    while (count < samples) {
        if (!imu_data_ready()) {
            sleep_ms(1);
            continue;
        }

        uint8_t raw[14];
        burst_read(REG_OUT_TEMP_L, raw, sizeof(raw));

        int16_t gx_raw = (int16_t)((raw[3] << 8) | raw[2]);
        int16_t gy_raw = (int16_t)((raw[5] << 8) | raw[4]);
        int16_t gz_raw = (int16_t)((raw[7] << 8) | raw[6]);

        float gx = ((float)gx_raw * GYRO_SENSITIVITY_MDPS_PER_LSB) / 1000.0f;
        float gy = ((float)gy_raw * GYRO_SENSITIVITY_MDPS_PER_LSB) / 1000.0f;
        float gz = ((float)gz_raw * GYRO_SENSITIVITY_MDPS_PER_LSB) / 1000.0f;

        sum_x += gx;
        sum_y += gy;
        sum_z += gz;

        count++;
    }

    gyro_bias[0] = sum_x / samples;
    gyro_bias[1] = sum_y / samples;
    gyro_bias[2] = sum_z / samples;

    printf("Gyro calibration complete, calculated bias: %.3f %.3f %.3f dps\n",
           gyro_bias[0], gyro_bias[1], gyro_bias[2]);
}

float imu_roll_deg(const imu_sample_t *imu) {
    return atan2f(imu->accel_g[1], imu->accel_g[2]) * RAD_TO_DEG;
}

float imu_pitch_deg(const imu_sample_t *imu) {
    return atan2f(-imu->accel_g[0], sqrtf(imu->accel_g[1] * imu->accel_g[1] +
                                          imu->accel_g[2] * imu->accel_g[2])) *
           RAD_TO_DEG;
}