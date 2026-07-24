add_library(gpio_control STATIC
    gpio_control.c
)
target_include_directories(gpio_control PUBLIC ${CMAKE_CURRENT_LIST_DIR})
target_link_libraries(gpio_control PUBLIC pico_stdlib hardware_gpio)
target_include_directories(gpio_control PUBLIC ${CMAKE_SOURCE_DIR}/include)
