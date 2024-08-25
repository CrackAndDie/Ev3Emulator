# Install script for directory: D:/Scripts/Ev3Emulator/Ev3LowLevel/lmssrc/Brick Datalog

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
   "/Ev3LowLevel/apps/Brick Datalog/Brick Datalog.rbf;/Ev3LowLevel/apps/Brick Datalog/144x82_POP4.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_PrtDetault.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_Rec.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_Set.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_RecH.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_SetH.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_Prt1.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_Prt2.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_Prt3.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_Prt4.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_PrtA.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_PrtB.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_PrtC.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_PrtD.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_Prt_All.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_Prt_Er.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_Prt_Un.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_count.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_mxave.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_NoDetails.rgf;/Ev3LowLevel/apps/Brick Datalog/dlSetAccept.rgf;/Ev3LowLevel/apps/Brick Datalog/dlSetAcceptH.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_GR0_1.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_GR0_8.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_GR0_10.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_GR0_100.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_GR0_250.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_GRc100.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_GRc180.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_GRc450.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_GR_TC.rgf;/Ev3LowLevel/apps/Brick Datalog/dl_Highrate.rgf;/Ev3LowLevel/apps/Brick Datalog/tdef.rgf;/Ev3LowLevel/apps/Brick Datalog/t010.rgf;/Ev3LowLevel/apps/Brick Datalog/t020.rgf;/Ev3LowLevel/apps/Brick Datalog/t030.rgf;/Ev3LowLevel/apps/Brick Datalog/t040.rgf;/Ev3LowLevel/apps/Brick Datalog/t050.rgf;/Ev3LowLevel/apps/Brick Datalog/t060.rgf;/Ev3LowLevel/apps/Brick Datalog/t061.rgf;/Ev3LowLevel/apps/Brick Datalog/t070.rgf;/Ev3LowLevel/apps/Brick Datalog/t071.rgf;/Ev3LowLevel/apps/Brick Datalog/t080.rgf;/Ev3LowLevel/apps/Brick Datalog/t081.rgf;/Ev3LowLevel/apps/Brick Datalog/t160.rgf;/Ev3LowLevel/apps/Brick Datalog/t290.rgf;/Ev3LowLevel/apps/Brick Datalog/t291.rgf;/Ev3LowLevel/apps/Brick Datalog/t292.rgf;/Ev3LowLevel/apps/Brick Datalog/t300.rgf;/Ev3LowLevel/apps/Brick Datalog/t320.rgf;/Ev3LowLevel/apps/Brick Datalog/t321.rgf;/Ev3LowLevel/apps/Brick Datalog/t330.rgf;/Ev3LowLevel/apps/Brick Datalog/t331.rgf;/Ev3LowLevel/apps/Brick Datalog/t332.rgf;/Ev3LowLevel/apps/Brick Datalog/PE_input.rgf;/Ev3LowLevel/apps/Brick Datalog/PE_output.rgf;/Ev3LowLevel/apps/Brick Datalog/icon.rgf;/Ev3LowLevel/apps/Brick Datalog/GeneralAlarm.rsf")
  if(CMAKE_WARN_ON_ABSOLUTE_INSTALL_DESTINATION)
    message(WARNING "ABSOLUTE path INSTALL DESTINATION : ${CMAKE_ABSOLUTE_DESTINATION_FILES}")
  endif()
  if(CMAKE_ERROR_ON_ABSOLUTE_INSTALL_DESTINATION)
    message(FATAL_ERROR "ABSOLUTE path INSTALL DESTINATION forbidden (by caller): ${CMAKE_ABSOLUTE_DESTINATION_FILES}")
  endif()
  file(INSTALL DESTINATION "/Ev3LowLevel/apps/Brick Datalog" TYPE FILE FILES
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/Brick Datalog.rbf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/144x82_POP4.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_PrtDetault.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_Rec.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_Set.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_RecH.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_SetH.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_Prt1.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_Prt2.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_Prt3.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_Prt4.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_PrtA.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_PrtB.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_PrtC.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_PrtD.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_Prt_All.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_Prt_Er.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_Prt_Un.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_count.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_mxave.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_NoDetails.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dlSetAccept.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dlSetAcceptH.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_GR0_1.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_GR0_8.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_GR0_10.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_GR0_100.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_GR0_250.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_GRc100.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_GRc180.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_GRc450.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_GR_TC.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/dl_Highrate.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/tdef.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t010.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t020.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t030.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t040.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t050.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t060.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t061.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t070.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t071.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t080.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t081.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t160.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t290.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t291.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t292.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t300.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t320.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t321.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t330.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t331.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/t332.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/PE_input.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/PE_output.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/icon.rgf"
    "D:/Scripts/Ev3Emulator/Ev3LowLevel/out/build/x86-debug/lmssrc/Brick Datalog/GeneralAlarm.rsf"
    )
endif()

