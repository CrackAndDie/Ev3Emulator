set (SOURCE_FILES
    c_bt.c
    c_com.c
    c_daisy.c
    c_i2c.c
    c_md5.c
    c_wifi.c
)

add_library (c_com OBJECT ${SOURCE_FILES})
target_include_directories (c_com PUBLIC
    ${CMAKE_CURRENT_BINARY_DIR}
    # ${CMAKE_SOURCE_DIR}/a_win
    ${CMAKE_SOURCE_DIR}/low_level
    ${CMAKE_SOURCE_DIR}/lms2012
    ${CMAKE_BINARY_DIR}/lms2012
    ${CMAKE_SOURCE_DIR}/c_input
    ${CMAKE_SOURCE_DIR}/c_memory
    ${CMAKE_SOURCE_DIR}/c_output
    ${LMS2012_DEPS_INCLUDE_DIRS}
)
