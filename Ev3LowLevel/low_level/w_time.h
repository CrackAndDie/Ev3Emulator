#ifndef   W_TIME_H_
#define   W_TIME_H_

// get ms
unsigned int (*w_time_getMs)(void);
__declspec(dllexport) void reg_w_time_getMs(unsigned int (*f)(void));

// get us
unsigned int (*w_time_getUs)(void);
__declspec(dllexport) void reg_w_time_getUs(unsigned int (*f)(void));

#endif // W_TIME_H_