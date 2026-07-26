#include "hid.h"
#include <stdlib.h>
#include <stdio.h>
#include "pico/stdlib.h"

#include "bsp/board_api.h"
#include "tusb.h"

#include "usb_descriptors.h"



// Invoked when received GET_REPORT control request
uint16_t tud_hid_get_report_cb(uint8_t instance, uint8_t report_id, hid_report_type_t report_type, uint8_t* buffer, uint16_t reqlen) {
    (void) instance;
    (void) report_id;
    (void) report_type;
    (void) buffer;
    (void) reqlen;
    return 0;
}

// Invoked when received SET_REPORT control request
void tud_hid_set_report_cb(uint8_t instance, uint8_t report_id, hid_report_type_t report_type, uint8_t const* buffer, uint16_t bufsize) {
    (void) instance;
    (void) report_id;
    (void) report_type;
    (void) buffer;
    (void) bufsize;
} 

void hid_init(void) {
    board_init();

    // init device stack on configured roothub port
    tusb_rhport_init_t dev_init = {.role = TUSB_ROLE_DEVICE, .speed = TUSB_SPEED_AUTO};
    tusb_init(BOARD_TUD_RHPORT, &dev_init);

    board_init_after_tusb();
}

void hid_update(int16_t x, int16_t y, bool clicking) {
    if (!tud_hid_ready()) return;

    uint8_t button = clicking ? 0x01 : 0x00;


    tud_hid_abs_mouse_report(REPORT_ID_MOUSE, button, x, y, 0, 0);
}
void hid_test(void){
    int16_t x[5] = {0, 32767, 0, 32767, 16383};
    int16_t y[5] = {0, 0, 32767, 32767, 16383};

    static uint8_t i = 0;
    hid_update(x[i], y[i], false);
    i++;
    if (i > 4) i = 0;
}