# Install script for directory: D:/Scripts/Ev3Emulator/Ev3LowLevel/lmssrc/Port View

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
   "/Ev3LowLevel/apps/Port View/Port View.rbf;/Ev3LowLevel/apps/Port View/ViewCore.rgf;/Ev3LowLevel/apps/Port View/ViewTop.rgf;/Ev3LowLevel/apps/Port View/ViewBot.rgf;/Ev3LowLevel/apps/Port View/ViewP1.rgf;/Ev3LowLevel/apps/Port View/ViewP2.rgf;/Ev3LowLevel/apps/Port View/ViewP3.rgf;/Ev3LowLevel/apps/Port View/ViewP4.rgf;/Ev3LowLevel/apps/Port View/ViewPA.rgf;/Ev3LowLevel/apps/Port View/ViewPB.rgf;/Ev3LowLevel/apps/Port View/ViewPC.rgf;/Ev3LowLevel/apps/Port View/ViewPD.rgf;/Ev3LowLevel/apps/Port View/tdef.rgf;/Ev3LowLevel/apps/Port View/t010.rgf;/Ev3LowLevel/apps/Port View/t020.rgf;/Ev3LowLevel/apps/Port View/t030.rgf;/Ev3LowLevel/apps/Port View/t040.rgf;/Ev3LowLevel/apps/Port View/t050.rgf;/Ev3LowLevel/apps/Port View/t060.rgf;/Ev3LowLevel/apps/Port View/t061.rgf;/Ev3LowLevel/apps/Port View/t070.rgf;/Ev3LowLevel/apps/Port View/t071.rgf;/Ev3LowLevel/apps/Port View/t080.rgf;/Ev3LowLevel/apps/Port View/t081.rgf;/Ev3LowLevel/apps/Port View/t160.rgf;/Ev3LowLevel/apps/Port View/t290.rgf;/Ev3LowLevel/apps/Port View/t291.rgf;/Ev3LowLevel/apps/Port View/t292.rgf;/Ev3LowLevel/apps/Port View/t300.rgf;/Ev3LowLevel/apps/Port View/t320.rgf;/Ev3LowLevel/apps/Port View/t321.rgf;/Ev3LowLevel/apps/Port View/t330.rgf;/Ev3LowLevel/apps/Port View/t331.rgf;/Ev3LowLevel/apps/Port View/t332.rgf;/Ev3LowLevel/apps/Port View/PE_input.rgf;/Ev3LowLevel/apps/Port View/PE_output.rgf;/Ev3LowLevel/apps/Port View/icon.rgf")
  if(CMAKE_WARN_ON_ABSOLUTE_INSTALL_DESTINATION)
    message(WARNING "ABSOLUTE path INSTALL DESTINATION : ${CMAKE_ABSOLUTE_DESTINATION_FILES}")
  endif()
  if(CMAKE_ERROR_ON_ABSOLUTE_INSTALL_DESTINATION)
    message(FATAL_ERROR "ABSOLUTE path INSTALL DESTINATION forbidden (by caller): ${CMAKE_ABSOLUTE_DESTINATION_FILES}")
  endif()
  file(INSTALL DESTINATION "/Ev3LowLevel/apps/Port View" TYPE FILE FILES
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/Port View.rbf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/ViewCore.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/ViewTop.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/ViewBot.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/ViewP1.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/ViewP2.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/ViewP3.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/ViewP4.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/ViewPA.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/ViewPB.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/ViewPC.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/ViewPD.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/tdef.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t010.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t020.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t030.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t040.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t050.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t060.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t061.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t070.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t071.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t080.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t081.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t160.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t290.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t291.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t292.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t300.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t320.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t321.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t330.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t331.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/t332.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/PE_input.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/PE_output.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/icon.rgf"
    )
endif()

