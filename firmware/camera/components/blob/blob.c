#include "blob.h"

#include <math.h>
#include <stdlib.h>
#include <string.h>

static point_t queue[QUEUE_SIZE];
static blob_t last_corners[4];
static bool have_last = false;

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
    set_pixel(image, start, 0);

    bool overflow = false;

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

                if (tail >= QUEUE_SIZE) {
                    overflow = true;
                    continue;
                }

                queue[tail++] = neighbour;
            }
        }
    }

    if (overflow) {
        return (blob_t){0};
    }

    return (blob_t){
        .row = (float)rowdA / (float)dA,
        .col = (float)coldA / (float)dA,
        .brightness = dA,
        .pixels = total_pixels,
    };
}

static void align_to_previous(blob_t blobs[4]) {
    if (!have_last) {
        have_last = true;
        memcpy(last_corners, blobs, sizeof(last_corners));
        return;
    }
    int best_shift = 0;
    float best_cost = INFINITY;
    for (int shift = 0; shift < 4; shift++) {
        float cost = 0;
        for (int i = 0; i < 4; i++) {
            blob_t *b = &blobs[(i + shift) % 4];
            float dr = b->row - last_corners[i].row;
            float dc = b->col - last_corners[i].col;
            cost += dr * dr + dc * dc;
        }
        if (cost < best_cost) {
            best_cost = cost;
            best_shift = shift;
        }
    }
    blob_t rotated[4];
    for (int i = 0; i < 4; i++)
        rotated[i] = blobs[(i + best_shift) % 4];
    memcpy(blobs, rotated, sizeof(rotated));
    memcpy(last_corners, blobs, sizeof(last_corners));
}

bool find_all_blobs(image_t image, blob_t best[4]) {
    int found = 0;

    for (int row = 0; row < ROWS; row += 2) {
        for (int col = 0; col < COLS; col += 2) {
            point_t coord = {.row = row, .col = col};

            if (get_pixel(image, coord) < THRESHOLD) {
                continue;
            }

            blob_t b = find_blob(image, coord);

            if (b.pixels < MIN_BLOB_SIZE) {
                continue;
            }

            insert_blob(best, &found, b);
        }
    }

    if (found != 4) {
        return false;
    }

    order_corners(best);
    /// this seems to break more things then it should plus orientation filtrer should hopefully replace it
    // align_to_previous(best);
    return true;
}