#pragma once

#include "pico/critical_section.h"
#include <stdbool.h>
#include <stdint.h>

typedef struct {
    bool trigger_pressed;
    uint32_t seq_number;
} shared_state_t;

extern critical_section_t g_state_lock;
extern shared_state_t g_global_state;

void shared_state_init();
shared_state_t shared_state_read();

void shared_state_update_trigger(bool trigger);
