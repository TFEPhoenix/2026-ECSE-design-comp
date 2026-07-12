#include "blob.h"

#include <math.h>
#include <stdlib.h>

static point_t queue[QUEUE_SIZE];

void insert_blob(blob_t best_four[4], int *found_so_far, blob_t new_blob) {
    if (*found_so_far < 4) {
        best_four[(*found_so_far)++] = new_blob;
        return;
    }

    // lowkey only 4 checks no need for whole loop or anything
    int dimmest = 0;
    if (best_four[1].brightness < best_four[dimmest].brightness)
        dimmest = 1;
    if (best_four[2].brightness < best_four[dimmest].brightness)
        dimmest = 2;
    if (best_four[3].brightness < best_four[dimmest].brightness)
        dimmest = 3;

    if (new_blob.brightness > best_four[dimmest].brightness) {
        best_four[dimmest] = new_blob;
    }
}

void order_corners(blob_t blobs[4]) {
    float cx =
        (blobs[0].col + blobs[1].col + blobs[2].col + blobs[3].col) / (float)4;
    float cy =
        (blobs[0].row + blobs[1].row + blobs[2].row + blobs[3].row) / (float)4;

    angle_blob_t a[4];
    for (int i = 0; i < 4; i++) {
        a[i].blob = blobs[i];
        a[i].angle = atan2f(blobs[i].row - cy, blobs[i].col - cx);
    }
    qsort(a, 4, sizeof(angle_blob_t), compare_angle);

    for (int i = 0; i < 4; i++) {
        blobs[i] = a[i].blob;
    }
}

blob_t find_blob(image_t image, point_t start) {
    static const int drow[DIRECTIONS] = {-1, -1, -1, 0, 0, 1, 1, 1};
    static const int dcol[DIRECTIONS] = {-1, 0, 1, -1, 1, 1, 0, -1};

    uint8_t start_pixel_value = get_pixel(image, start);

    int total_pixels = 1;
    int dA = start_pixel_value;
    int rowdA = start.row * start_pixel_value;
    int coldA = start.col * start_pixel_value;

    int head = 0;
    int tail = 0;

    queue[tail++] = start;

    if (tail >= QUEUE_SIZE) {
        return (blob_t){0, 0, 0, 0};
    }

    set_pixel(image, start, 0);

    while (head != tail) {
        point_t node = queue[head++];

        for (int i = 0; i < DIRECTIONS; i++) {
            int nrow = node.row + drow[i];
            int ncol = node.col + dcol[i];

            if (ncol >= 0 && ncol < COLS && nrow >= 0 && nrow < ROWS) {
                point_t neighbour = (point_t){.row = nrow, .col = ncol};
                uint8_t npixel_value = get_pixel(image, neighbour);
                if (npixel_value < THRESHOLD) {
                    continue;
                }

                total_pixels++;
                dA += npixel_value;
                rowdA += neighbour.row * npixel_value;
                coldA += neighbour.col * npixel_value;

                set_pixel(image, neighbour, 0);
                queue[tail++] = neighbour;
            }
        }
    }

    return (blob_t){
        .row = (float)rowdA / (float)dA,
        .col = (float)coldA / (float)dA,
        .brightness = dA,
        .pixels = total_pixels,
    };
}

bool find_all_blobs(image_t image, blob_t best[4]) {
    int found = 0;
    for (int row = 0; row < ROWS; row++) {
        for (int col = 0; col < COLS; col++) {
            point_t coord = {.row = row, .col = col};

            if (get_pixel(image, coord) < THRESHOLD) {
                continue;
            }

            blob_t b = find_blob(image, coord);

            if (b.pixels < MIN_BLOB_SIZE) {
                continue; // likely noise or smth like that
            }

            insert_blob(best, &found, b);
        }
    }

    if (found != 4)
        return false;

    order_corners(best);
    return true;
}