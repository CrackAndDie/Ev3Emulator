# Install script for directory: D:/Scripts/Ev3Emulator/Ev3LowLevel/lmssrc/ui

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
   "/Ev3LowLevel/sys/ui/ui.rbf;/Ev3LowLevel/sys/ui/mindstorms.rgf;/Ev3LowLevel/sys/ui/Ani1x.rgf;/Ev3LowLevel/sys/ui/screen.rgf;/Ev3LowLevel/sys/ui/playrecent_screen.rgf;/Ev3LowLevel/sys/ui/file_screen.rgf;/Ev3LowLevel/sys/ui/apps_screen.rgf;/Ev3LowLevel/sys/ui/settings_screen.rgf;/Ev3LowLevel/sys/ui/144x65_POP3.rgf;/Ev3LowLevel/sys/ui/144x82_POP4.rgf;/Ev3LowLevel/sys/ui/Click.rsf;/Ev3LowLevel/sys/ui/DownloadSuccess.rsf;/Ev3LowLevel/sys/ui/GeneralAlarm.rsf;/Ev3LowLevel/sys/ui/OverpowerAlert.rsf;/Ev3LowLevel/sys/ui/PowerDown.rsf;/Ev3LowLevel/sys/ui/Startup.rsf")
  if(CMAKE_WARN_ON_ABSOLUTE_INSTALL_DESTINATION)
    message(WARNING "ABSOLUTE path INSTALL DESTINATION : ${CMAKE_ABSOLUTE_DESTINATION_FILES}")
  endif()
  if(CMAKE_ERROR_ON_ABSOLUTE_INSTALL_DESTINATION)
    message(FATAL_ERROR "ABSOLUTE path INSTALL DESTINATION forbidden (by caller): ${CMAKE_ABSOLUTE_DESTINATION_FILES}")
  endif()
  file(INSTALL DESTINATION "/Ev3LowLevel/sys/ui" TYPE FILE FILES
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/ui/ui.rbf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/ui/mindstorms.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/ui/Ani1x.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/ui/screen.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/ui/playrecent_screen.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/ui/file_screen.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/ui/apps_screen.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/ui/settings_screen.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/ui/144x65_POP3.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/ui/144x82_POP4.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/ui/Click.rsf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/ui/DownloadSuccess.rsf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/ui/GeneralAlarm.rsf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/ui/OverpowerAlert.rsf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/ui/PowerDown.rsf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/ui/Startup.rsf"
    )
endif()

