#include "one_euro_filter.h"
#include <math.h>

static float low_pass(float alpha, float x, float x_prev) {
    return alpha * x + (1.0f - alpha) * x_prev;
}

static float smoothing_alpha(float cutoff_hz, float dt) {
    float tau = 1.0f / (2.0f * (float)M_PI * cutoff_hz);
    return 1.0f / (1.0f + tau / dt);
}

void one_euro_init(one_euro_filter_t *f, float min_cutoff, float beta,
                   float d_cutoff) {
    f->min_cutoff = min_cutoff;
    f->beta = beta;
    f->d_cutoff = d_cutoff;
    f->initialized = false;
    f->x_prev = 0.0f;
    f->dx_prev = 0.0f;
}

float one_euro_filter(one_euro_filter_t *f, float x, float dt) {
    if (!f->initialized) {
        f->x_prev = x;
        f->dx_prev = 0.0f;
        f->initialized = true;
        return x;
    }

    float dx = (x - f->x_prev) / dt;
    float a_d = smoothing_alpha(f->d_cutoff, dt);
    float dx_hat = low_pass(a_d, dx, f->dx_prev);

    float cutoff = f->min_cutoff + f->beta * fabsf(dx_hat);

    float a = smoothing_alpha(cutoff, dt);
    float x_hat = low_pass(a, x, f->x_prev);

    f->x_prev = x_hat;
    f->dx_prev = dx_hat;

    return x_hat;
}