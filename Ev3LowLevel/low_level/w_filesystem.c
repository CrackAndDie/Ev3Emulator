#include "w_filesystem.h"
#include <stdlib.h>

void reg_w_filesystem_getUsedMemory(int (*f)(void))
{
    w_filesystem_getUsedMemory = f;
}