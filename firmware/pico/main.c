#include "gpio_control.h"
#include "pico/stdlib.h"
#include "uart_camera.h"

int main() {
    stdio_init_all();
    // main_uart();

    gpio_loop();
}
