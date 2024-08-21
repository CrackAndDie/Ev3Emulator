#ifndef   W_MOTORS_H_
#define   W_MOTORS_H_

// get busy flags
void (*w_motors_getBusyFlags)(int* f1, int* f2);
__declspec(dllexport) void reg_w_motors_getBusyFlags(void (*f)(int* f1, int* f2));

// set busy flags
void (*w_motors_setBusyFlags)(int f1);
__declspec(dllexport) void reg_w_motors_setBusyFlags(void (*f)(int f1));

// set data
void (*w_motors_setData)(char* data, int len);
__declspec(dllexport) void reg_w_motors_setData(void (*f)(char* data, int len));

#endif // W_MOTORS_H_