# CMakeList.txt : CMake project for Ev3LowLevel, include source and define
# project specific logic here.
#

set (SOURCE_FILES
    dirent_win.c
)

add_library (win OBJECT ${SOURCE_FILES})

