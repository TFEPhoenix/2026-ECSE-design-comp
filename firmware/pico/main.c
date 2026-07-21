#include "gpio_control.h"
#include "pico/stdlib.h"
#include "uart_camera.h"

int main() {
    stdio_init_all();
    io_init();
    main_uart();
}
