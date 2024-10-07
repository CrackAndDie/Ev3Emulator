#ifndef   W_SYSTEM_H_
#define   W_SYSTEM_H_

__declspec(dllexport) void w_system_startMain(void);

// to get if the vm should be stopped from outside
unsigned char (*w_system_getStopMain)(void);
__declspec(dllexport) void reg_w_system_getStopMain(unsigned char (*f)(void));

// ------- helpers

int w_system_printf(const char* format, ...);

void w_system_sleep_ms(int milliseconds); // cross-platform sleep function

char* w_system_replaceWord(const char* s, const char* oldW, const char* newW);

#endif // W_SYSTEM_H_