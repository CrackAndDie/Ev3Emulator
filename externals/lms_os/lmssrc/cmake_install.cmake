# Install script for directory: D:/Scripts/Ev3Emulator/Ev3LowLevel/lmssrc

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

if(NOT CMAKE_INSTALL_LOCAL_ONLY)
  # Include the install script for each subdirectory.
  include("D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Bluetooth/cmake_install.cmake")
  include("D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/cmake_install.cmake")
  include("D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Info/cmake_install.cmake")
  include("D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Name/cmake_install.cmake")
  include("D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Program/cmake_install.cmake")
  include("D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Debug/cmake_install.cmake")
  include("D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Demo/cmake_install.cmake")
  include("D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/IR Control/cmake_install.cmake")
  include("D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Motor Control/cmake_install.cmake")
  include("D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Port View/cmake_install.cmake")
  include("D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/SelfTest/cmake_install.cmake")
  include("D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Sleep/cmake_install.cmake")
  include("D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/TEST/cmake_install.cmake")
  include("D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Volume/cmake_install.cmake")
  include("D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Wifi/cmake_install.cmake")
  include("D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/tst/cmake_install.cmake")
  include("D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/ui/cmake_install.cmake")

endif()

