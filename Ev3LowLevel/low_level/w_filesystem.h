
#ifndef   W_FILESYSTEM_H_
#define   W_FILESYSTEM_H_

// create dir
int (*w_filesystem_createDir)(const char* name);
__declspec(dllexport) void reg_w_filesystem_createDir(int (*f)(const char* name));

#endif // W_FILESYSTEM_H_