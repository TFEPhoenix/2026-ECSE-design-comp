#pragma once

#include <stdbool.h>

typedef struct {
    float min_cutoff;
    float beta;
    float d_cutoff;
    bool initialized;
    float x_prev;
    float dx_prev;
} one_euro_filter_t;

void one_euro_init(one_euro_filter_t *f, float min_cutoff, float beta,
                   float d_cutoff);
float one_euro_filter(one_euro_filter_t *f, float x, float t_seconds);