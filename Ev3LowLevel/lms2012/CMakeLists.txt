
set (SOURCE_FILES
    c_branch.c
    c_compare.c
    c_math.c
    c_move.c
    c_timer.c
    lms2012.c
    validate.c
    bytecodes.c
)

set (LMS2012_MODULES
    low_level
    # a_win
    c_com
    c_dynload
    c_input
    c_memory
    c_output
    c_robotcvm
    c_sound
    c_ui
)

foreach (module ${LMS2012_MODULES})
    list (APPEND LMS2012_OBJECTS "$<TARGET_OBJECTS:${module}>")
    list (APPEND LMS2012_INCLUDE_DIRS ${CMAKE_SOURCE_DIR}/${module})
    list (APPEND LMS2012_INCLUDE_DIRS ${CMAKE_SOURCE_DIR}/${module}/source)
endforeach ()

add_library (lms2012 SHARED ${SOURCE_FILES} ${LMS2012_OBJECTS})
target_include_directories (lms2012 PUBLIC
    ${CMAKE_CURRENT_SOURCE_DIR}
    ${CMAKE_CURRENT_BINARY_DIR}
    ${LMS2012_INCLUDE_DIRS}
    ${LMS2012_DEPS_INCLUDE_DIRS}
)
if (CMAKE_CROSSCOMPILING)
    # These are not picked up automatically when cross compiling for some reason
    target_link_libraries (lms2012 "-lresolv")
endif (CMAKE_CROSSCOMPILING)

install (TARGETS lms2012 RUNTIME DESTINATION ${CMAKE_INSTALL_SBINDIR})
