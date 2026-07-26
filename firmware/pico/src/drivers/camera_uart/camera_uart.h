#include <stdbool.h>
#include <stdint.h>

typedef struct {
    float col;
    float row;
    bool found;
    float dist_m;
} camera_sample_t;

void camera_uart_init();
bool camera_uart_get_sample(camera_sample_t *out);