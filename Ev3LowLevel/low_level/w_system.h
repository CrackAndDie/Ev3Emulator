#ifndef   W_SYSTEM_H_
#define   W_SYSTEM_H_

__declspec(dllexport) void w_system_startMain(void);
__declspec(dllexport) void w_system_stopMain(void);

// ------- helpers

int w_system_printf(const char* format, ...);

#endif // W_SYSTEM_H_