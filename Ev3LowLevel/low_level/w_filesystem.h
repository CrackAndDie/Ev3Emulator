
#ifndef   W_FILESYSTEM_H_
#define   W_FILESYSTEM_H_

struct FILESYSTEM_ENTITY {
	// is this directory
	unsigned char isDir;
	// for dir to enumerate items
	unsigned char searchOffset;
	// 0 - success, 1 - error ....
	unsigned char result;
	// 0 - not exists, 1 - exists
	unsigned char exists;

	// just a name without any path shite!!!
	char* name;
};
typedef struct FILESYSTEM_ENTITY FILESYSTEM_ENTITY;

// create dir. ret 0 if ok
int (*w_filesystem_createDir)(const char* name);
__declspec(dllexport) void reg_w_filesystem_createDir(int (*f)(const char* name));

// gets the next element in directory and returns it
FILESYSTEM_ENTITY (*w_filesystem_readDir)(FILESYSTEM_ENTITY dir);
__declspec(dllexport) void reg_w_filesystem_readDir(FILESYSTEM_ENTITY (*f)(FILESYSTEM_ENTITY dir));

// returns all elements in dir, amount. sort 0 - no sort, sort 1 - sort, sort 2 - alphasort
// return value should be allocated in c# and freed in c
FILESYSTEM_ENTITY* (*w_filesystem_scanDir)(const char* name, int* amount, unsigned char sort);
__declspec(dllexport) void reg_w_filesystem_scanDir(FILESYSTEM_ENTITY* (*f)(const char* name, int* amount, unsigned char sort));

// get used memory in KB
int (*w_filesystem_getUsedMemory)(void);
__declspec(dllexport) void reg_w_filesystem_getUsedMemory(int (*f)(void));

// sync filesystem. probably no need
void (*w_filesystem_sync)(void);
__declspec(dllexport) void reg_w_filesystem_sync(void (*f)(void));

// --------- helper methods

FILESYSTEM_ENTITY* w_filesystem_openDir(const char* folderName);
void w_filesystem_closeDir(FILESYSTEM_ENTITY* folder);

#endif // W_FILESYSTEM_H_