#pragma once

#include <stdbool.h>
#include <stdint.h>

#define THRESHOLD 128
#define ROWS 240
#define COLS 320
#define IMAGE_SIZE (ROWS * COLS)
#define DIRECTIONS 8

typedef struct {
    float row;
    float col;
} point_t;

typedef struct {
    float row;
    float col;
    int brightness;
    int pixels;
} blob_t;

typedef struct {
    blob_t blob;
    float angle;
} angle_blob_t;

typedef enum { TOP_LEFT = 0, TOP_RIGHT, BOTTOM_RIGHT, BOTTOM_LEFT } corner_t;

typedef uint8_t image_t[ROWS * COLS];

static inline uint8_t get_pixel(image_t image, point_t coord) {
    return image[((int)coord.row * COLS) + (int)coord.col];
}
static inline void set_pixel(image_t image, point_t coord, uint8_t value) {
    image[((int)coord.row * COLS) + (int)coord.col] = value;
}
static inline int compare_angle(const void *a, const void *b) {
    return (((const angle_blob_t *)a)->angle >
            ((const angle_blob_t *)b)->angle) -
           (((const angle_blob_t *)a)->angle <
            ((const angle_blob_t *)b)->angle);
}

void order_corners(blob_t blobs[4]);
blob_t find_blob(image_t image, point_t start);
bool find_all_blobs(image_t image, blob_t best[4]);
void insert_blob(blob_t best_four[4], int *found_so_far, blob_t new_blob);