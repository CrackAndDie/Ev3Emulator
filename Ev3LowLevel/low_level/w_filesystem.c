#include "w_filesystem.h"
#include <stdlib.h>

void reg_w_filesystem_createDir(int (*f)(const char* name))
{
    w_filesystem_createDir = f;
}

void reg_w_filesystem_readDir(FILESYSTEM_ENTITY(*f)(const char* name, int offset))
{
	w_filesystem_readDir = f;
}

void reg_w_filesystem_scanDir(FILESYSTEM_ENTITY* (*f)(const char* name, int* amount, unsigned char sort))
{
	w_filesystem_scanDir = f;
}

void reg_w_filesystem_getUsedMemory(int (*f)(void))
{
    w_filesystem_getUsedMemory = f;
}

void reg_w_filesystem_sync(void (*f)(void))
{
	w_filesystem_sync = f;
}

// ----- helpers

FILESYSTEM_ENTITY* w_filesystem_openDir(const char* folderName)
{
	struct FILESYSTEM_ENTITY* pDir;
	pDir = (struct FILESYSTEM_ENTITY*)malloc(sizeof(struct FILESYSTEM_ENTITY));
	pDir->isDir = 1;
	pDir->exists = 1;
	pDir->result = 0;
	pDir->searchOffset = 0;
	pDir->name = folderName;
	return pDir;
}

void w_filesystem_closeDir(FILESYSTEM_ENTITY* folder)
{
	free(folder);
}