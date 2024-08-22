# CMakeList.txt : CMake project for Ev3LowLevel, include source and define
# project specific logic here.
#

set (SOURCE_FILES
    dirent_win.c
)

add_library (a_win OBJECT ${SOURCE_FILES})
target_include_directories (a_win PUBLIC
    ${CMAKE_SOURCE_DIR}/lms2012
    ${CMAKE_BINARY_DIR}/lms2012
    ${LMS2012_DEPS_INCLUDE_DIRS}
)
