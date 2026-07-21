#include "hardware/uart.h"
#include "pico/stdlib.h"
#include <stdio.h>

#define UART_ID uart1
#define UART_TX_PIN 8
#define UART_RX_PIN 9
#define BAUD_RATE 115200

static volatile char buffer[128];
static volatile int buffer_idx = 0;

void on_uart_rx() {
    while (uart_is_readable(UART_ID)) {
        char c = uart_getc(UART_ID);

        if (c == '\n' || buffer_idx >= sizeof(buffer) - 1) {
            buffer[buffer_idx] = '\0';
            buffer_idx = 0;
            printf("Received: %s\n", buffer);
        } else {
            buffer[buffer_idx++] = c;
        }
    }
}

void uart_camera_init() {
    buffer_idx = 0;

    uart_init(UART_ID, BAUD_RATE);

    gpio_set_function(UART_TX_PIN, GPIO_FUNC_UART);
    gpio_set_function(UART_RX_PIN, GPIO_FUNC_UART);

    uart_set_hw_flow(UART_ID, false, false);
    uart_set_format(UART_ID, 8, 1, UART_PARITY_NONE);

    uart_set_fifo_enabled(UART_ID, false);
    irq_set_exclusive_handler(UART1_IRQ, on_uart_rx);
    irq_set_enabled(UART1_IRQ, true);
    uart_set_irq_enables(UART_ID, true, false);
}