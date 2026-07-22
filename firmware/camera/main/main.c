#include "blob.h"
#include "camera_config.h"
#include "homography.h"

#include "esp_camera.h"
#include "freertos/FreeRTOS.h"

void app_main(void) {
    if (camera_init() != ESP_OK) {
        return;
    }

    camera_fb_t *fb;
    point_t position = {0};
    blob_t best[4];

    while (1) {
        fb = esp_camera_fb_get();
        if (!fb) {
            continue;
        }

        bool found = find_all_blobs(fb->buf, best);
        if (found) {
            point_t center = {fb->height / 2.0, fb->width / 2.0};

            homography_t h = compute_heckbert_h(best);
            homography_t h_inv = invert_homography(h);

            position = apply_homography(&h_inv, center);
        }

        printf("POS,%.4f,%.4f,%d\n", position.col, position.row, found ? 1 : 0);
        esp_camera_fb_return(fb);
    }
}