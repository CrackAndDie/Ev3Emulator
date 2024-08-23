#ifndef   ANOTHER_H_
#define   ANOTHER_H_

struct FILESYSTEM_ENTITY {
	// is this directory
	unsigned char isDir;
	// for dir to enumerate items
	unsigned char searchOffset;
	// 0 - success, 1 - error ....
	unsigned char result;
	// 0 - not exists, 1 - exists
	unsigned char exists;
};
typedef struct FILESYSTEM_ENTITY FILESYSTEM_ENTITY;



__declspec(dllexport) void getCringe(int* abb);
__declspec(dllexport) void getCringe222(int* abb);
__declspec(dllexport) void getCringe333(void);

// gets the next element in directory and returns it
FILESYSTEM_ENTITY(*w_filesystem_readDir)(FILESYSTEM_ENTITY dir);
__declspec(dllexport) void reg_w_filesystem_readDir(FILESYSTEM_ENTITY(*f)(FILESYSTEM_ENTITY dir));

void (*w_filesystem_readDir222)(unsigned char* buff, int len);
__declspec(dllexport) void reg_w_filesystem_readDir222(void (*f)(unsigned char* buff, int len));

// TODO: Reference additional headers your program requires here.

#endif /* ANOTHER_H_ */