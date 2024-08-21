# CMakeList.txt : CMake project for Ev3LowLevel, include source and define
# project specific logic here.
#

set (SOURCE_FILES
    w_filesystem.c
)

add_library (low_level OBJECT ${SOURCE_FILES})

