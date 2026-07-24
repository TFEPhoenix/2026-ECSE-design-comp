#include <stdint.h>
#include <stdbool.h>

// Called to start the HID
void hid_init();

// Called everytime there is a cursor movement or button press.
void hid_update(uint16_t x, uint16_t y, bool clicking);