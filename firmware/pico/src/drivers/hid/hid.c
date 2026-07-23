#include "hid.h"
#include <stdlib.h>
#include <stdio.h>
#include "pico/stdlib.h"

#include "bsp/board_api.h"
#include "tusb.h"

#include "usb_descriptors.h"

void hid_init(void) {
    board_init();

    // init device stack on configured roothub port
    tusb_rhport_init_t dev_init = {.role = TUSB_ROLE_DEVICE, .speed = TUSB_SPEED_AUTO};
    tusb_init(BOARD_TUD_RHPORT, &dev_init);

    board_init_after_tusb();
}

void hid_update(uint16_t x, uint16_t y, bool clicking) {
    if (!tud_hid_ready()) return;

    hid_stylus_report_t report;
    report.x = x;
    report.y = y;
    
    // Click when the switch is trigger is pulled
    report.attr = clicking ? STYLUS_ATTR_TIP_SWITCH | STYLUS_ATTR_IN_RANGE : STYLUS_ATTR_IN_RANGE; 

    tud_hid_report(REPORT_ID_STYLUS, &report, sizeof(report));
}