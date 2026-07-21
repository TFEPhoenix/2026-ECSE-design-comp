#pragma once

#include <stdbool.h>
#include <stdint.h>

typedef struct {
    float col;
    float row;
    bool found;
} camera_sample_t;

void uart_camera_init();
bool uart_camera_get_sample(camera_sample_t *out);