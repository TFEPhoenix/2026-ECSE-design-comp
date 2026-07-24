#pragma once

#include "pico/critical_section.h"
#include <stdbool.h>
#include <stdint.h>

typedef struct {
    bool trigger_pressed;
    uint32_t seq_number;
    uint16_t x, y; // coordernates scaled by 2^15 - 1
} shared_state_t;

extern critical_section_t g_state_lock;
extern shared_state_t g_global_state;

void shared_state_init();
shared_state_t shared_state_read();

void shared_state_update_trigger(bool trigger);
void shared_state_update_coords(uint16_t x, uint16_t y);