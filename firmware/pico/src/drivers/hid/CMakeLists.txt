add_library(hid STATIC
    hid.c
)
target_include_directories(hid PUBLIC ${CMAKE_CURRENT_LIST_DIR})
target_link_libraries(hid PUBLIC pico_stdlib tinyusb_device tinyusb_board)