#include "blob.h"
#include "camera_config.h"
#include "driver/uart.h"
#include "esp_camera.h"
#include "freertos/FreeRTOS.h"
#include "homography.h"
#include <string.h>

#define UART_TX 13
#define UART_RX 12
#define UART_PORT UART_NUM_1
#define BUF_SIZE 1024

bool init_uart();

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

        char message[256];
        sprintf(message, "POS,%.4f,%.4f,%d\n", position.col, position.row,
                found ? 1 : 0);

        uart_write_bytes(UART_PORT, message, strlen(message));
        esp_camera_fb_return(fb);
    }
}

bool init_uart() {
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
