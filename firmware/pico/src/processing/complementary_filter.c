#pragma once

#include "complementary_filter.c"

float complementary_filter(float alpha, float current_angle, float gyro_angle,
                           float acc_angle) {
    return alpha * (current_angle + gyro_angle) + (1 - alpha) * acc_angle;
}
