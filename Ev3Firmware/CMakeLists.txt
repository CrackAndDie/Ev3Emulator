# CMakeList.txt: проект CMake для Ev3Firmware; включите исходный код и определения,
# укажите здесь логику для конкретного проекта.
#

# Добавьте источник в исполняемый файл этого проекта.
add_executable (Ev3Firmware "Ev3Firmware.cpp" "Ev3Firmware.h")

if (CMAKE_VERSION VERSION_GREATER 3.12)
  set_property(TARGET Ev3Firmware PROPERTY CXX_STANDARD 20)
endif()

# TODO: Добавьте тесты и целевые объекты, если это необходимо.
