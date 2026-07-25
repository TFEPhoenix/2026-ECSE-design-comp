add_library(filters STATIC
    complementary_filter.c one_euro_filter.c orientation_filter.c
)
target_include_directories(filters PUBLIC ${CMAKE_CURRENT_LIST_DIR})
target_link_libraries(filters PUBLIC pico_stdlib)
target_link_libraries(filters PUBLIC imu_spi camera_uart)
