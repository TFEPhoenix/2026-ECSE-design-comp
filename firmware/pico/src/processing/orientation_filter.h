#pragma once

#include "imu_spi.h"
#include <stdbool.h>

typedef struct {
    float roll_deg;
    float pitch_deg;
    float yaw_deg;
    float alpha;
    bool initialized;
} orientation_filter_t;

void orientation_filter_init(orientation_filter_t *f, float alpha);
void orientation_filter_update(orientation_filter_t *f, const imu_sample_t *imu,
                               float dt);
void orientation_filter_reset(orientation_filter_t *f);