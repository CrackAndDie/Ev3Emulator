
macro (add_lms_app TARGET)
    set (options)
    set (one_value_args ARCHIVE INSTALL_PATH ICON)
    set (multi_value_args PROGRAMS PROGRAMS_TASM GRAPHICS SOUNDS)
    cmake_parse_arguments (LMSSRC
        "${options}"
        "${one_value_args}"
        "${multi_value_args}"
        ${ARGN}
    )
    if (LMSSRC_ICON)
        list (APPEND LMSSRC_GRAPHICS icon)
        set (icon_SOURCE ${LMSSRC_ICON})
    endif ()

    foreach (program ${LMSSRC_PROGRAMS})
        set (lms ${CMAKE_CURRENT_SOURCE_DIR}/${program}.lms)
        set (rbf ${CMAKE_CURRENT_BINARY_DIR}/${program}.rbf)

        configure_file(${program}.rbf ${program}.rbf COPYONLY)

         # removed converter command

        list (APPEND LMSSRC_RBF_LIST ${rbf})
    endforeach ()

    foreach (graphic ${LMSSRC_GRAPHICS})
        # some generated files do not have the same name as the source file
        set (source ${${graphic}_SOURCE})
        if (NOT source)
            set (source ${graphic})
        endif ()

        set (bmp ${CMAKE_CURRENT_SOURCE_DIR}/${source}.bmp)
        set (rgf ${CMAKE_CURRENT_BINARY_DIR}/${graphic}.rgf)

        configure_file(${graphic}.rgf ${graphic}.rgf COPYONLY)

        # removed converter command

        list (APPEND LMSSRC_RGF_LIST ${rgf})
    endforeach ()

    foreach (sound ${LMSSRC_SOUNDS})
        set (wav ${CMAKE_CURRENT_SOURCE_DIR}/${sound}.wav)
        set (raw ${CMAKE_CURRENT_BINARY_DIR}/${sound}.raw)
        set (rsf ${CMAKE_CURRENT_BINARY_DIR}/${sound}.rsf)

        configure_file(${sound}.raw ${sound}.raw COPYONLY)
        configure_file(${sound}.rsf ${sound}.rsf COPYONLY)

        # removed converter command

        list (APPEND LMSSRC_RSF_LIST ${rsf})
    endforeach ()



endmacro ()

# add_subdirectory (Bluetooth)
# add_subdirectory ("Brick Datalog")
# add_subdirectory ("Brick Info")
# add_subdirectory ("Brick Name")
# add_subdirectory ("Brick Program")
# add_subdirectory (Debug)
# add_subdirectory (Demo)
# add_subdirectory ("IR Control")
# add_subdirectory ("Motor Control")
# add_subdirectory ("Port View")
# add_subdirectory (SelfTest)
# add_subdirectory (Sleep)
# add_subdirectory (TEST)
# add_subdirectory (Volume)
# add_subdirectory (WiFi)
# add_subdirectory (tst)
# add_subdirectory (ui)


# custom shite
add_subdirectory (prjs)
add_subdirectory (apps)
add_subdirectory (tools)
add_subdirectory (sys)
