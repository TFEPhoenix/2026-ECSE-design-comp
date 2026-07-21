#include "hardware/uart.h"
#include "pico/stdlib.h"
#include <stdio.h>

#define UART_ID uart1
#define UART_TX_PIN 8
#define UART_RX_PIN 9
#define BAUD_RATE 115200

void uart_camera_init() {
    uart_init(UART_ID, BAUD_RATE);

    gpio_set_function(UART_TX_PIN, GPIO_FUNC_UART);
    gpio_set_function(UART_RX_PIN, GPIO_FUNC_UART);

    uart_set_hw_flow(UART_ID, false, false);
    uart_set_format(UART_ID, 8, 1, UART_PARITY_NONE);
}

int main_uart() {
    char buffer[128];
    int buffer_idx = 0;

    while (true) {
        if (!uart_is_readable(UART_ID))
            continue;

        char c = uart_getc(UART_ID);

        if (c == '\n') {
            buffer[buffer_idx] = '\0';
            printf("Received: %s\n", buffer);
            buffer_idx = 0;
        } else if (buffer_idx >= sizeof(buffer) - 1) {
            buffer[buffer_idx] = '\0';
            buffer_idx = 0;
            printf("Buffer Overflowed, received: %s\n", buffer);
        }

        buffer[buffer_idx++] = c;
    }

    return 0;
}