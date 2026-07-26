#include "blob.h"
#include "camera_config.h"
#include "driver/uart.h"
#include "esp_camera.h"
#include "freertos/FreeRTOS.h"
#include "homography.h"
#include <math.h>
#include <string.h>

#define UART_TX 13
#define UART_RX 12
#define UART_PORT UART_NUM_1
#define BUF_SIZE 1024

#define CAMERA_FOCAL_PX 246.0f // using the ol' formula f = d/2tan(fov/2)
#define LED_TOP_WIDTH_M 0.50f // distance between tl and tr in m

static bool init_uart();
static float estimate_distance_m(blob_t corners[4]);

void app_main(void) {
    if (!init_uart()) {
        return;
    }

    if (camera_init() != ESP_OK) {
        return;
    }

    camera_fb_t *fb;
    point_t position = {0};
    blob_t best[4];
    float distance_m = 1.0f;

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
            distance_m = estimate_distance_m(best);
        }

        char message[256];
        sprintf(message, "POS,%.4f,%.4f,%d,%.4f\n", position.col, position.row,
                found ? 1 : 0, distance_m);

        uart_write_bytes(UART_PORT, message, strlen(message));
        esp_camera_fb_return(fb);
    }
}

static bool init_uart() {
    uart_config_t uart_config = {
        .baud_rate = 115200,
        .data_bits = UART_DATA_8_BITS,
        .parity = UART_PARITY_DISABLE,
        .stop_bits = UART_STOP_BITS_1,
        .flow_ctrl = UART_HW_FLOWCTRL_DISABLE,
    };
    int intr_alloc_flags = 0;

    esp_err_t e1 =
        uart_driver_install(UART_PORT, BUF_SIZE, 0, 0, NULL, intr_alloc_flags);
    esp_err_t e2 = uart_param_config(UART_PORT, &uart_config);
    esp_err_t e3 = uart_set_pin(UART_PORT, UART_TX, UART_RX, -1, -1);

    return e1 == ESP_OK && e2 == ESP_OK && e3 == ESP_OK;
}

static float estimate_distance_m(blob_t corners[4]) {
    float dx = corners[TOP_RIGHT].col - corners[TOP_LEFT].col;
    float dy = corners[TOP_RIGHT].row - corners[TOP_LEFT].row;
    float pixel_spacing = sqrtf(dx * dx + dy * dy);

    if (pixel_spacing < 1.0f) {
        pixel_spacing = 1.0f;
    }

    return CAMERA_FOCAL_PX * LED_TOP_WIDTH_M / pixel_spacing;
}