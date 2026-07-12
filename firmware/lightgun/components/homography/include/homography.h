#pragma once
#include "blob.h"

#define BASICALLY_ZERO 1e-8f

typedef struct {
    float m[3][3];
} homography_t;

homography_t compute_heckbert_h(blob_t corners[4]);
homography_t invert_homography(homography_t H);
point_t apply_homography(const homography_t *H, point_t p);