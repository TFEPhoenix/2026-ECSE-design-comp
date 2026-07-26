# Generated Cmake Pico project file

cmake_minimum_required(VERSION 3.13)

set(CMAKE_C_STANDARD 11)
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)

# Initialise pico_sdk from installed location
# (note this can come from environment, CMake cache etc)

# == DO NOT EDIT THE FOLLOWING LINES for the Raspberry Pi Pico VS Code Extension to work ==
# set(sdkVersion 2.3.0)
set(toolchainVersion 15_2_Rel1)
set(picotoolVersion 2.3.0)
set(picoVscode ${USERHOME}/.pico-sdk/cmake/pico-vscode.cmake)
if(EXISTS ${picoVscode})
    include(${picoVscode})
endif()
# ====================================================================================
set(PICO_BOARD pico CACHE STRING "Board type")

# Pull in Raspberry Pi Pico SDK (must be before project)
include(pico_sdk_import.cmake)

project(pico C CXX ASM)

# Initialise the Raspberry Pi Pico SDK
pico_sdk_init()

add_subdirectory(src/drivers/camera_uart)
add_subdirectory(src/drivers/imu_spi)
add_subdirectory(src/drivers/gpio_control)
add_subdirectory(src/processing)
# add_subdirectory(src/drivers/hid)

add_executable(pico
    main.c
    src/core1/core1.c
    src/shared_state.c
)

pico_set_program_name(pico "pico")
pico_set_program_version(pico "0.1")

pico_enable_stdio_uart(pico 0)
pico_enable_stdio_usb(pico 1) # for testing purposes ill leave this on until phoenix adds his tinyusb code

target_include_directories(pico PRIVATE
    ${CMAKE_CURRENT_LIST_DIR}
    ${CMAKE_CURRENT_LIST_DIR}/include
    ${CMAKE_CURRENT_LIST_DIR}/src/core1
)

target_link_libraries(pico
    pico_stdlib
    pico_multicore)


target_link_libraries(pico
    hardware_spi
    hardware_timer
    camera_uart
    imu_spi
    gpio_control
    filters
    # hid
)

pico_add_extra_outputs(pico)
