# CMakeList.txt : CMake project for TestProject, include source and define
# project specific logic here.
#

# Add source to this project's executable.
add_library (TestProject SHARED "TestProject.c" "TestProject.h"  )
# add_definitions(/fsanitize=address /Zi)

set(CMAKE_CXX_STANDARD 11)

# Set the public header property to the one with the actual API.
# set_target_properties(${PROJECT_NAME} PROPERTIES PUBLIC_HEADER TestProject.h)

# TODO: Add tests and install targets if needed.
