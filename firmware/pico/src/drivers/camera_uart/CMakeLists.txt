add_library(camera_uart STATIC
    camera_uart.c
)
target_include_directories(camera_uart PUBLIC ${CMAKE_CURRENT_LIST_DIR})
target_link_libraries(camera_uart PUBLIC pico_stdlib hardware_uart)
