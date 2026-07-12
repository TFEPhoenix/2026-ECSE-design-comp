#include "camera_config.h"
#include "camera_pinout.h"
#include "esp_camera.h"
#include "esp_log.h"

static const camera_config_t camera_config_s = {
    .pin_pwdn = CAM_PIN_PWDN,
    .pin_reset = CAM_PIN_RESET,
    .pin_xclk = CAM_PIN_XCLK,
    .pin_sccb_sda = CAM_PIN_SIOD,
    .pin_sccb_scl = CAM_PIN_SIOC,

    .pin_d7 = CAM_PIN_D7,
    .pin_d6 = CAM_PIN_D6,
    .pin_d5 = CAM_PIN_D5,
    .pin_d4 = CAM_PIN_D4,
    .pin_d3 = CAM_PIN_D3,
    .pin_d2 = CAM_PIN_D2,
    .pin_d1 = CAM_PIN_D1,
    .pin_d0 = CAM_PIN_D0,
    .pin_vsync = CAM_PIN_VSYNC,
    .pin_href = CAM_PIN_HREF,
    .pin_pclk = CAM_PIN_PCLK,

    .xclk_freq_hz = 20000000,
    .ledc_timer = LEDC_TIMER_0,
    .ledc_channel = LEDC_CHANNEL_0,

    .pixel_format = PIXFORMAT_GRAYSCALE,
    .frame_size = FRAMESIZE_QVGA,

    .jpeg_quality = 0,
    .fb_count = 2,
    .fb_location = CAMERA_FB_IN_PSRAM,
    .grab_mode = CAMERA_GRAB_LATEST,
};

static void camera_configure_sensor() {
    sensor_t *s = esp_camera_sensor_get();

    s->set_framesize(s, FRAMESIZE_QVGA);
    s->set_quality(s, 0);

    s->set_brightness(s, 0);
    s->set_contrast(s, 3);
    s->set_saturation(s, -4);
    s->set_sharpness(s, 3);
    s->set_denoise(s, 8);
    s->set_special_effect(s, 2); // should be greyscale

    s->set_aec_value(s, 300);
    s->set_agc_gain(s, 20);
    s->set_gainceiling(s, GAINCEILING_2X);

    // Disable all the auto shit
    s->set_whitebal(s, 0);
    s->set_awb_gain(s, 0);
    s->set_wb_mode(s, 0);
    s->set_exposure_ctrl(s, 0);
    s->set_aec2(s, 0);
    s->set_gain_ctrl(s, 0);
    s->set_bpc(s, 0);
    s->set_wpc(s, 0);
    s->set_lenc(s, 0);
    s->set_hmirror(s, 0);
    s->set_vflip(s, 0);
    s->set_colorbar(s, 0);
}

esp_err_t camera_init() {
    esp_err_t err = esp_camera_init(&camera_config_s);

    if (err != ESP_OK) {
        ESP_LOGE("CAMERA", "Camera Init Failed");
        return err;
    }

    camera_configure_sensor();

    return ESP_OK;
}

esp_err_t camera_deinit() { return esp_camera_deinit(); }