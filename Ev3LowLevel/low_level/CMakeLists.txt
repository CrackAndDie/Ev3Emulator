# CMakeList.txt : CMake project for Ev3LowLevel, include source and define
# project specific logic here.
#

set (SOURCE_FILES
    w_system.c
    w_filesystem.c
    w_time.c
    w_lcd.c
    w_buttons.c
    w_motors.c
    w_sound.c
    w_input.c
)

add_library (low_level OBJECT ${SOURCE_FILES})
target_include_directories (low_level PUBLIC
    ${CMAKE_SOURCE_DIR}/lms2012
    ${CMAKE_BINARY_DIR}/lms2012
    ${LMS2012_DEPS_INCLUDE_DIRS}
)
