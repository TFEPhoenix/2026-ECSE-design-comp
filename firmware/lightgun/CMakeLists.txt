cmake_minimum_required(VERSION 3.16)
include($ENV{IDF_PATH}/tools/cmake/project.cmake)

add_compile_definitions("BOARD_ESP32CAM_AITHINKER")

project(lightgun)