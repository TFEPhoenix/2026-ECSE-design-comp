#include "orientation_filter.h"
#include "complementary_filter.c"
#include <math.h>

#define DEG_TO_RAD (float)(M_PI / 180.0)

void orientation_filter_init(orientation_filter_t *f, float alpha) {
    f->roll_deg = 0.0f;
    f->pitch_deg = 0.0f;
    f->yaw_deg = 0.0f;
    f->alpha = alpha;
    f->initialized = false;
}

void orientation_filter_update(orientation_filter_t *f, const imu_sample_t *imu,
                               float dt) {
    if (!f->initialized) {
        f->roll_deg = imu_roll_deg(imu);
        f->initialized = true;
    }

    // Calculate total acceleration magnitude
    float accel_mag = sqrtf(imu->accel_g[0] * imu->accel_g[0] +
                            imu->accel_g[1] * imu->accel_g[1] +
                            imu->accel_g[2] * imu->accel_g[2]);

    // During a fast flick, G-forces spike. If we aren't near 1G, rely purely on
    // the gyro (alpha = 1.0f) to prevent horizontal acceleration from tricking
    // the filter into thinking the gun rolled.
    float roll_alpha = (accel_mag > 0.8f && accel_mag < 1.2f) ? f->alpha : 1.0f;

    f->roll_deg = complementary_filter(
        roll_alpha, f->roll_deg, imu->gyro_dps[0] * dt, imu_roll_deg(imu));

    float roll_rad = f->roll_deg * DEG_TO_RAD;
    float cos_r = cosf(roll_rad);
    float sin_r = sinf(roll_rad);

    float gy = imu->gyro_dps[1]; 
    float gz = imu->gyro_dps[2];

    float pitch_rate = gy * cos_r - gz * sin_r;
    float yaw_rate = gy * sin_r + gz * cos_r;

    f->pitch_deg += pitch_rate * dt;
    f->yaw_deg += yaw_rate * dt;
}

void orientation_filter_reset(orientation_filter_t *f) {
    f->pitch_deg = 0.0f;
    f->yaw_deg = 0.0f;
}