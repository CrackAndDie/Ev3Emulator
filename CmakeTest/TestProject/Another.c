#include "Another.h"
#include <stdlib.h>

void getCringe(int* abb) {
	*abb = sizeof(int*) + 7;
}

void getCringe222(int* abb) {
	FILESYSTEM_ENTITY ent;
	ent.searchOffset = 0;

	w_filesystem_readDir(ent);
	*abb = ent.searchOffset;
}

void getCringe333(void) {
	unsigned char* bf = (unsigned char*)malloc(13);
	w_filesystem_readDir222(bf, 13);
}

void reg_w_filesystem_readDir(FILESYSTEM_ENTITY(*f)(FILESYSTEM_ENTITY dir))
{
	w_filesystem_readDir = f;
}

void reg_w_filesystem_readDir222(void (*f)(unsigned char* buff, int len))
{
	w_filesystem_readDir222 = f;
}