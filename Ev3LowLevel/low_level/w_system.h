#ifndef   W_SYSTEM_H_
#define   W_SYSTEM_H_

__declspec(dllexport) void w_system_startMain(void);

// to get if the vm should be stopped from outside
unsigned char (*w_system_getStopMain)(void);
__declspec(dllexport) void reg_w_system_getStopMain(unsigned char (*f)(void));

// ------- helpers

int w_system_printf(const char* format, ...);

#endif // W_SYSTEM_H_