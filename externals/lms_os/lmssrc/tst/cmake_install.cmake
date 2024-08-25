# Install script for directory: D:/Scripts/Ev3Emulator/Ev3LowLevel/lmssrc/tst

# Set the install prefix
if(NOT DEFINED CMAKE_INSTALL_PREFIX)
  set(CMAKE_INSTALL_PREFIX "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/install/x86-debug")
endif()
string(REGEX REPLACE "/$" "" CMAKE_INSTALL_PREFIX "${CMAKE_INSTALL_PREFIX}")

# Set the install configuration name.
if(NOT DEFINED CMAKE_INSTALL_CONFIG_NAME)
  if(BUILD_TYPE)
    string(REGEX REPLACE "^[^A-Za-z0-9_]+" ""
           CMAKE_INSTALL_CONFIG_NAME "${BUILD_TYPE}")
  else()
    set(CMAKE_INSTALL_CONFIG_NAME "Debug")
  endif()
  message(STATUS "Install configuration: \"${CMAKE_INSTALL_CONFIG_NAME}\"")
endif()

# Set the component getting installed.
if(NOT CMAKE_INSTALL_COMPONENT)
  if(COMPONENT)
    message(STATUS "Install component: \"${COMPONENT}\"")
    set(CMAKE_INSTALL_COMPONENT "${COMPONENT}")
  else()
    set(CMAKE_INSTALL_COMPONENT)
  endif()
endif()

# Is this installation the result of a crosscompile?
if(NOT DEFINED CMAKE_CROSSCOMPILING)
  set(CMAKE_CROSSCOMPILING "FALSE")
endif()

if(CMAKE_INSTALL_COMPONENT STREQUAL "Unspecified" OR NOT CMAKE_INSTALL_COMPONENT)
  list(APPEND CMAKE_ABSOLUTE_DESTINATION_FILES
   "/Ev3LowLevel/sys/tst/tst.rbf;/Ev3LowLevel/sys/tst/Performance.rbf;/Ev3LowLevel/sys/tst/tststr.rbf;/Ev3LowLevel/sys/tst/tstmath.rbf;/Ev3LowLevel/sys/tst/us.rbf;/Ev3LowLevel/sys/tst/si.rbf;/Ev3LowLevel/sys/tst/tstlog.rbf;/Ev3LowLevel/sys/tst/tstiic.rbf")
  if(CMAKE_WARN_ON_ABSOLUTE_INSTALL_DESTINATION)
    message(WARNING "ABSOLUTE path INSTALL DESTINATION : ${CMAKE_ABSOLUTE_DESTINATION_FILES}")
  endif()
  if(CMAKE_ERROR_ON_ABSOLUTE_INSTALL_DESTINATION)
    message(FATAL_ERROR "ABSOLUTE path INSTALL DESTINATION forbidden (by caller): ${CMAKE_ABSOLUTE_DESTINATION_FILES}")
  endif()
  file(INSTALL DESTINATION "/Ev3LowLevel/sys/tst" TYPE FILE FILES
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/tst/tst.rbf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/tst/Performance.rbf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/tst/tststr.rbf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/tst/tstmath.rbf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/tst/us.rbf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/tst/si.rbf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/tst/tstlog.rbf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/tst/tstiic.rbf"
    )
endif()

