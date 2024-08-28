# CMakeList.txt : Top-level CMake project file, do global configuration
# and include sub-projects here.
#
cmake_minimum_required (VERSION 3.8)

set(CMAKE_CXX_STANDARD 11)

project (Ev3LowLevel VERSION 3.22 LANGUAGES C)

set (LMS2012_SPECIALVERS "COMPAT")
set (LMS2012_DEBUG_UART_PORT 0)
set (LMS2012_ENABLE_TERMINAL 0)
set (LMS2012_DEBUG_BUFPRINT 0)
set (LMS2012_ENABLE_DEBUG_PULSE 0)

add_definitions ("-DLinux_X86")
add_definitions ("-DDEBUG")
add_definitions ("-DDEBUG_TO_FILE")

add_definitions("-D${LMS2012_SPECIALVERS}VERS")
if (LMS2012_ENABLE_TERMINAL)
    add_definitions ("-DTERMINAL_ENABLED=1")
else ()
    add_definitions ("-DTERMINAL_ENABLED=0")
endif ()
# subtracting one here converts port number to enum value
add_definitions ("-DDEBUG_UART=(${LMS2012_DEBUG_UART_PORT}-1)")
set (LMS2012_DEBUG_OPTIONS
    MEMORY_USAGE

    VM
    TRACE_TASK
    C_BT
    C_COM
    C_INPUT
    C_INPUT_DAISYCHAIN
    C_INPUT_DATALOG
    C_INPUT_FAST_DATALOG
    C_MEMORY
    C_MEMORY_LOG
    C_MEMORY_FILE
    C_MEMORY_LOW
    C_SOUND
    C_UI
    TRACE_MODE_CHANGE
    TRACE_KEY
    TRACE_VM
    TRACE_DAISYCHAIN
    BYTECODE_TIME
    TRACE_FREEZE
    TRACE_FILENAME
    SDCARD
    USBSTICK
    VIRTUAL_BATT_TEMP
    TEMP_SHUTDOWN
    BACK_BLOCKED
    MEMORY_USAGE
    RECHARGABLE
    WIFI
)
foreach (DEBUG_OPTION ${LMS2012_DEBUG_OPTIONS})
    if (LMS2012_DEBUG_${DEBUG_OPTION})
        add_definitions ("-DDEBUG_${DEBUG_OPTION}")
    endif ()
endforeach ()
if (LMS2012_DEBUG_BUFPRINT)
    add_definitions ("-DBUFPRINTSIZE=100000")
endif ()
if (LMS2012_ENABLE_DEBUG_PULSE)
    add_definitions ("-DALLOW_DEBUG_PULSE")
endif ()
set (LMS2012_ENABLE_OPTIONS
    OLDCALL
    LOG_ASCII
    HIGH_CURRENT
    PERFORMANCE_TEST
    LOAD_TEST
    MEMORY_TEST
    STATUS_TEST
)
foreach (OPTION ${LMS2012_ENABLE_OPTIONS})
    if (LMS2012_ENABLE_${OPTION})
        add_definitions ("-DENABLE_${OPTION}")
    endif ()
endforeach ()
set (LMS2012_DISABLE_OPTIONS
    OLD_COLOR
    DAISYCHAIN
    DAISYCHAIN_COM_CALL
    FAST_DATALOG_BUFFER
    BUMPED
    LOW_VOLTAGE
    LOW_MEMORY
    UART_DATA_ERROR
    SDCARD_SUPPORT
    USBSTICK_SUPPORT
    VIRTUAL_BATT_TEMP
    SOUND
    PAR_ALIGNMENT
    NEW_CALL_MUTEX
    SYSTEM_BYTECODE
    FILENAME_CHECK
    AD_WORD_PROTECT
    UPDATE_DISASSEMBLY
    BLOCK_ALIAS_LOCALS
)
foreach (OPTION ${LMS2012_DISABLE_OPTIONS})
    if (NOT LMS2012_ENABLE_${OPTION})
        add_definitions ("-DDISABLE_${OPTION}")
    endif ()
endforeach () 

# global compiler options

add_definitions("-DPROJECT=\"${PROJECT_NAME}\"" "-DVERS=${PROJECT_VERSION}")
add_compile_options (-Wall)

# uncomment to enable bound checks
# add_definitions(/fsanitize=address)

# Include sub-projects.
add_subdirectory (low_level)
# add_subdirectory (a_win)
add_subdirectory (c_com)
add_subdirectory (c_dynload)
add_subdirectory (c_input)
add_subdirectory (c_memory)
add_subdirectory (c_output)
add_subdirectory (c_robotcvm)
add_subdirectory (c_sound)
add_subdirectory (c_ui)
add_subdirectory (data)
# add_subdirectory (doc)
add_subdirectory (lms2012)
add_subdirectory (lmssrc)
