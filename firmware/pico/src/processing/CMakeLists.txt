add_library(filters STATIC
    filters.c
)
target_include_directories(filters PUBLIC ${CMAKE_CURRENT_LIST_DIR})
target_link_libraries(filters PUBLIC pico_stdlib)
target_link_libraries(filters PUBLIC imu_spi camera_uart)
