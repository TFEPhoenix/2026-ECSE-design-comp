#include "blob.h"
#include "camera_config.h"
#include "homography.h"

#include "esp_camera.h"
#include "freertos/FreeRTOS.h"
#include "freertos/queue.h"
#include "freertos/task.h"

#define SCREEN_WIDTH 1920
#define SCREEN_HEIGHT 1080

static QueueHandle_t frame_queue;

static void capture_task(void *arg);
static void process_task(void *arg);

void app_main(void) {
    if (camera_init() != ESP_OK) {
        return;
    }
    // camera_test();

    frame_queue = xQueueCreate(1, sizeof(camera_fb_t *));

    xTaskCreatePinnedToCore(capture_task, "capture", 4096, NULL, 5, NULL, 0);
    xTaskCreatePinnedToCore(process_task, "process", 8192, NULL, 5, NULL, 1);
}

static void capture_task(void *arg) {
    while (1) {
        camera_fb_t *fb = esp_camera_fb_get();
        if (!fb) {
            continue;
        }

        camera_fb_t *old_fb = NULL;
        // if my other task didnt already process the buffer, return it here
        if (xQueuePeek(frame_queue, &old_fb, 0) == pdTRUE) {
            xQueueReceive(frame_queue, &old_fb, 0);
            esp_camera_fb_return(old_fb);
        }

        xQueueSend(frame_queue, &fb, 0);
    }
}

static void process_task(void *arg) {
    camera_fb_t *fb;
    point_t position = {0};
    blob_t best[4];

    while (1) {
        if (xQueueReceive(frame_queue, &fb, portMAX_DELAY) == pdTRUE) {
            bool found = find_all_blobs(fb->buf, best);

            if (found) {
                // printf("Blobs Found: ");
                // for (int i = 0; i < 4; i++) {
                //     printf("(%f, %f, %i, %i), ", best[i].col, best[i].row,
                //            best[i].pixels, best[i].brightness);
                // }
                // printf("\n");

                point_t center = {120, 160};
                homography_t h = compute_heckbert_h(best);
                homography_t h_inv = invert_homography(h);
                position = apply_homography(&h_inv, center);
            }

            // ESP_LOGI("GUN POSITION", "The gun is pointing at: (%f, %f)",
            //          position.col * SCREEN_WIDTH,
            //          (1 - position.row) * SCREEN_HEIGHT);

            printf("POS,%.4f,%.4f,%d\n", position.col, position.row,
                   found ? 1 : 0);

            esp_camera_fb_return(fb);
        }
    }
}