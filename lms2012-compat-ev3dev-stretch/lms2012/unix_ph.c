#include  "unix_ph.h"
#include <cstddef>
#include <stdlib.h>
#include <stdio.h>
#include <direct.h>

void sync(void) { }

int chmod(const char *path, int mode) { return 0; }
int mkdir_cst(const char *path, int mode) 
{ 
    _mkdir(path);
    // CreateDirectory(path, NULL);
    return 0; 
}
