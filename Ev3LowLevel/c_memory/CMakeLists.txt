
set (SOURCE_FILES
    c_memory.c
)

add_library (c_memory OBJECT ${SOURCE_FILES})
target_include_directories (c_memory PUBLIC
    # ${CMAKE_SOURCE_DIR}/a_win
    ${CMAKE_SOURCE_DIR}/low_level
    ${CMAKE_SOURCE_DIR}/lms2012
    ${CMAKE_BINARY_DIR}/lms2012
    ${CMAKE_SOURCE_DIR}/c_com
    ${CMAKE_SOURCE_DIR}/c_ui
)
