
set (SOURCE_FILES
    c_sound.c
)

add_library (c_sound OBJECT ${SOURCE_FILES})
target_include_directories (c_sound PUBLIC
    # ${CMAKE_SOURCE_DIR}/a_win
    ${CMAKE_SOURCE_DIR}/low_level
    ${CMAKE_SOURCE_DIR}/lms2012
    ${CMAKE_BINARY_DIR}/lms2012
    ${LMS2012_DEPS_INCLUDE_DIRS}
)
