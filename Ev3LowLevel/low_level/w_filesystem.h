
#ifndef   W_FILESYSTEM_H_
#define   W_FILESYSTEM_H_

// get used memory in KB
int (*w_filesystem_getUsedMemory)(void);
__declspec(dllexport) void reg_w_filesystem_getUsedMemory(int (*f)(void));

#endif // W_FILESYSTEM_H_