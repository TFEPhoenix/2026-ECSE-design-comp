#include "uart_camera.h"
#include <hardware/sync.h>
#include "hardware/uart.h"
#include <string.h>
#include "pico/stdlib.h"
#include <stdio.h>

#define UART_ID uart1
#define UART_TX_PIN 8
#define UART_RX_PIN 9
#define BAUD_RATE 115200

static volatile char buffer[128];
static volatile int buffer_idx = 0;

static volatile bool sample_ready = false;
static volatile camera_sample_t latest_sample;

static void parse_and_store(const char *line) {
    float col, row;
    int found_flag;

    if (sscanf(line, "POS,%f,%f,%d", &col, &row, &found_flag) == 3) {
        latest_sample.col = col;
        latest_sample.row = row;
        latest_sample.found = (found_flag != 0);
        sample_ready = true;
    }
}

void on_uart_rx() {
    while (uart_is_readable(UART_ID)) {
        char c = uart_getc(UART_ID);

        if (c == '\n' || buffer_idx >= (int)sizeof(buffer) - 1) {
            char local_copy[128];
            memcpy(local_copy, (const char *)buffer, buffer_idx);
            local_copy[buffer_idx] = '\0';
            buffer_idx = 0;
            parse_and_store(local_copy);
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

bool uart_camera_get_sample(camera_sample_t *out) {
    if (!sample_ready)
        return false;

    uint32_t saved_irq = save_and_disable_interrupts();
    *out = (camera_sample_t){latest_sample.col, latest_sample.row,
                             latest_sample.found};
    sample_ready = false;
    restore_interrupts(saved_irq);

    return true;
}