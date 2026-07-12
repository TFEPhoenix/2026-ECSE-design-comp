#include "homography.h"
#include "blob.h"
#include <math.h>

// TODO: generalise the homopraphy struct
// TODO: reimplement w/ gaussian elimination

homography_t compute_heckbert_h(blob_t corners[4]) {
    // essentially implements:
    // https://pages.cs.wisc.edu/~dyer/cs766/readings/heckbert-proj.pdf
    float x0 = corners[BOTTOM_LEFT].col;
    float x1 = corners[BOTTOM_RIGHT].col;
    float x2 = corners[TOP_RIGHT].col;
    float x3 = corners[TOP_LEFT].col;

    float y0 = corners[BOTTOM_LEFT].row;
    float y1 = corners[BOTTOM_RIGHT].row;
    float y2 = corners[TOP_RIGHT].row;
    float y3 = corners[TOP_LEFT].row;

    float dx1 = x1 - x2;
    float dx2 = x3 - x2;
    float dy1 = y1 - y2;
    float dy2 = y3 - y2;

    float sx = x0 - x1 + x2 - x3;
    float sy = y0 - y1 + y2 - y3;

    float denom = dx1 * dy2 - dx2 * dy1;

    homography_t H;

    if (fabsf(denom) < BASICALLY_ZERO) {
        // case a from the paper
        H.m[0][0] = x1 - x0;
        H.m[0][1] = x3 - x0;
        H.m[0][2] = x0;

        H.m[1][0] = y1 - y0;
        H.m[1][1] = y3 - y0;
        H.m[1][2] = y0;

        H.m[2][0] = 0.0f;
        H.m[2][1] = 0.0f;
        H.m[2][2] = 1.0f;

        return H;
    }

    float g = (sx * dy2 - dx2 * sy) / denom;
    float h = (dx1 * sy - sx * dy1) / denom;

    H.m[0][0] = x1 - x0 + g * x1;
    H.m[0][1] = x3 - x0 + h * x3;
    H.m[0][2] = x0;

    H.m[1][0] = y1 - y0 + g * y1;
    H.m[1][1] = y3 - y0 + h * y3;
    H.m[1][2] = y0;

    H.m[2][0] = g;
    H.m[2][1] = h;
    H.m[2][2] = 1;

    return H;
}

homography_t invert_homography(homography_t H) {
    float a = H.m[0][0];
    float b = H.m[0][1];
    float c = H.m[0][2];

    float d = H.m[1][0];
    float e = H.m[1][1];
    float f = H.m[1][2];

    float g = H.m[2][0];
    float h = H.m[2][1];
    float i = H.m[2][2];

    float det = a * (e * i - f * h) - b * (d * i - f * g) + c * (d * h - e * g);
    float inv = 1 / det;

    homography_t R;

    R.m[0][0] = (e * i - f * h) * inv;
    R.m[0][1] = -(b * i - c * h) * inv;
    R.m[0][2] = (b * f - c * e) * inv;

    R.m[1][0] = -(d * i - f * g) * inv;
    R.m[1][1] = (a * i - c * g) * inv;
    R.m[1][2] = -(a * f - c * d) * inv;

    R.m[2][0] = (d * h - e * g) * inv;
    R.m[2][1] = -(a * h - b * g) * inv;
    R.m[2][2] = (a * e - b * d) * inv;

    return R;
}

point_t apply_homography(const homography_t *H, point_t p) {
    float w = H->m[2][0] * p.col + H->m[2][1] * p.row + H->m[2][2];

    return (point_t){
        .col = (H->m[0][0] * p.col + H->m[0][1] * p.row + H->m[0][2]) / w,
        .row = (H->m[1][0] * p.col + H->m[1][1] * p.row + H->m[1][2]) / w};
}

point_t apply_transformation(point_t p) {
    const homography_t T = {.m = {{0.5, 0.5, 0}, {-0.5, 0.5, 0.5}, {0, 0, 0}}};
    return apply_homography(&T, p);
}