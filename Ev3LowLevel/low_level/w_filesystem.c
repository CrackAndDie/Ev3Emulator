#include "w_filesystem.h"

void reg_w_filesystem_createDir(int (*f)(const char* name))
{
    w_filesystem_createDir = f;
}