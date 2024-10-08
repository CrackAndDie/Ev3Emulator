﻿# CMakeList.txt : CMake project for Ev3LowLevel, include source and define
# project specific logic here.
#

set (SOURCE_FILES
    browser.c
    button.c
    c_ui.c
    d_lcd.c
    d_terminal.c
    dialog.c
    draw.c
    graph.c
    keyboard.c
    led.c
    power.c
    textbox.c
)


add_library (c_ui OBJECT ${SOURCE_FILES})
target_include_directories (c_ui PUBLIC
    ${CMAKE_SOURCE_DIR}/low_level
    ${CMAKE_SOURCE_DIR}/lms2012
    ${CMAKE_BINARY_DIR}/lms2012
    ${CMAKE_SOURCE_DIR}/c_com
    ${CMAKE_SOURCE_DIR}/c_input
    ${CMAKE_SOURCE_DIR}/c_memory
    ${CMAKE_CURRENT_BINARY_DIR}
    ${LMS2012_DEPS_INCLUDE_DIRS}
)

set (C_UI_ICONS
    small_icons
    menu_icons
    normal_icons
    large_icons
    arrow_icons
    small_font
    normal_font
    large_font
    tiny_font
    mindstorms
    Ani1x
    Ani2x
    Ani3x
    Ani4x
    Ani5x
    Ani6x
    POP3
)
set (BROWSER_ICONS
    PCApp
)
set (DIALOG_ICONS
    POP2
    POP3
)
set (KEYBOARD_ICONS
    keyboardc
    keyboardn
    keyboards
)

# TODO: Would be nice to rename icons so we don't have to have this lookup table
set (small_icons_SOURCE     16x8_TopBar_Icons)
set (menu_icons_SOURCE      16x12_MenuItems)
set (normal_icons_SOURCE    24x12_Files_Folders_Settings)
set (large_icons_SOURCE     24x22_Yes_No_OFF_FILEOps)
set (arrow_icons_SOURCE     8x12_miniArrows)
set (small_font_SOURCE      brick_font_2011_v1.3-bold)
set (normal_font_SOURCE     brick_font_2011_v1.3-regular)
set (large_font_SOURCE      brick_font_LARGE)
set (tiny_font_SOURCE       small_font)
set (keyboardc_SOURCE       KEY_CAPCHAR)
set (keyboardn_SOURCE       KEY_NumSymb)
set (keyboards_SOURCE       KEY_SmCHAR)
set (mindstorms_SOURCE      mindstorms)
set (POP2_SOURCE            144x48_POP2)
set (POP3_SOURCE            144x65_POP3)
set (PCApp_SOURCE           24x12_PCApp)

macro (bmp_to_xbm GROUP)
    foreach (icon ${${GROUP}_ICONS})
        set (source ${${icon}_SOURCE})
        if (NOT source)
            set (source ${icon})
        endif ()
        # set (bmp ${CMAKE_CURRENT_SOURCE_DIR}/bmps/${source}.bmp)
        set (xbm ${CMAKE_CURRENT_BINARY_DIR}/${icon}.xbm)

        configure_file(xbms/${icon}.xbm ${icon}.xbm COPYONLY)

        # add_custom_command (OUTPUT ${xbm}
            # COMMAND ${IMAGEMAGICK_CONVERT} ${bmp} ${xbm}
            # DEPENDS ${bmp}
        # )
        list (APPEND ${GROUP}_XBMS ${xbm})
    endforeach ()
endmacro ()

bmp_to_xbm (BROWSER)
bmp_to_xbm (C_UI)
bmp_to_xbm (DIALOG)
bmp_to_xbm (KEYBOARD)

# these .c files include xbm files inline
set_source_files_properties (browser.c PROPERTIES OBJECT_DEPENDS "${BROWSER_XBMS}")
set_source_files_properties (c_ui.c PROPERTIES OBJECT_DEPENDS "${C_UI_XBMS}")
set_source_files_properties (dialog.c PROPERTIES OBJECT_DEPENDS "${DIALOG_XBMS}")
set_source_files_properties (keyboard.c PROPERTIES OBJECT_DEPENDS "${KEYBOARD_XBMS}")
