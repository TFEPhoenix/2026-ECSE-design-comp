#include "shared_state.h"

critical_section_t g_state_lock;
shared_state_t g_global_state;

void shared_state_init() { critical_section_init(&g_state_lock); }

shared_state_t shared_state_read() {
    critical_section_enter_blocking(&g_state_lock);
    shared_state_t copy = g_global_state;
    critical_section_exit(&g_state_lock);
    return copy;
}

void shared_state_update_trigger(bool trigger) {
    critical_section_enter_blocking(&g_state_lock);
    g_global_state.trigger_pressed = trigger;
    g_global_state.seq_number++;
    critical_section_exit(&g_state_lock);
}
