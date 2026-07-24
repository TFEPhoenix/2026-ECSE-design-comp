add_library(imu_spi STATIC
    imu_spi.c
)
target_include_directories(imu_spi PUBLIC ${CMAKE_CURRENT_LIST_DIR})
target_link_libraries(imu_spi PUBLIC pico_stdlib hardware_spi)
