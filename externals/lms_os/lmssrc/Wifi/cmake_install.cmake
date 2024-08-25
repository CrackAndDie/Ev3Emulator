# Install script for directory: D:/Scripts/Ev3Emulator/Ev3LowLevel/lmssrc/Wifi

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
   "/Ev3LowLevel/tools/WiFi/WiFi.rbf;/Ev3LowLevel/tools/WiFi/144x48_POP2.rgf;/Ev3LowLevel/tools/WiFi/144x65_POP3.rgf;/Ev3LowLevel/tools/WiFi/144x82_POP4.rgf;/Ev3LowLevel/tools/WiFi/144x99_POP5.rgf;/Ev3LowLevel/tools/WiFi/144x116_POP6.rgf;/Ev3LowLevel/tools/WiFi/icon.rgf;/Ev3LowLevel/tools/WiFi/GeneralAlarm.rsf;/Ev3LowLevel/tools/WiFi/Connect.rsf")
  if(CMAKE_WARN_ON_ABSOLUTE_INSTALL_DESTINATION)
    message(WARNING "ABSOLUTE path INSTALL DESTINATION : ${CMAKE_ABSOLUTE_DESTINATION_FILES}")
  endif()
  if(CMAKE_ERROR_ON_ABSOLUTE_INSTALL_DESTINATION)
    message(FATAL_ERROR "ABSOLUTE path INSTALL DESTINATION forbidden (by caller): ${CMAKE_ABSOLUTE_DESTINATION_FILES}")
  endif()
  file(INSTALL DESTINATION "/Ev3LowLevel/tools/WiFi" TYPE FILE FILES
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Wifi/WiFi.rbf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Wifi/144x48_POP2.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Wifi/144x65_POP3.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Wifi/144x82_POP4.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Wifi/144x99_POP5.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Wifi/144x116_POP6.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Wifi/icon.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Wifi/GeneralAlarm.rsf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Wifi/Connect.rsf"
    )
endif()

