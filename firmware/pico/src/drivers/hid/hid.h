#include <stdint.h>
#include <stdbool.h>

#include "bsp/board_api.h"
#include "tusb.h"

#include "usb_descriptors.h"

// Called to start the HID
void hid_init();

// Small test for the HID
void hid_test();

// Called everytime there is a cursor movement or button press.
void hid_update(int16_t x, int16_t y, bool clicking);