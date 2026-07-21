add_library(uart_camera STATIC
    uart_camera.c
)
target_include_directories(uart_camera PUBLIC ${CMAKE_CURRENT_LIST_DIR})
target_link_libraries(uart_camera PUBLIC pico_stdlib hardware_uart)
