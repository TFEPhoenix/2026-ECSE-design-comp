#include "camera_config.h"

void app_main(void) {
    if (camera_init() != ESP_OK) {
        return;
    }
}