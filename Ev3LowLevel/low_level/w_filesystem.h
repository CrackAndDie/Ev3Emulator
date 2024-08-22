
#ifndef   W_FILESYSTEM_H_
#define   W_FILESYSTEM_H_

// create dir. ret 0 if ok
int (*w_filesystem_createDir)(const char* name);
__declspec(dllexport) void reg_w_filesystem_createDir(int (*f)(const char* name));

// get used memory in KB
int (*w_filesystem_getUsedMemory)(void);
__declspec(dllexport) void reg_w_filesystem_getUsedMemory(int (*f)(void));

#endif // W_FILESYSTEM_H_