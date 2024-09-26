#ifndef   W_MOTORS_H_
#define   W_MOTORS_H_

#include "lms2012.h"

// get busy flags
void (*w_motors_getBusyFlags)(int* f1, int* f2);
__declspec(dllexport) void reg_w_motors_getBusyFlags(void (*f)(int* f1, int* f2));

// set busy flags
void (*w_motors_setBusyFlags)(int f1);
__declspec(dllexport) void reg_w_motors_setBusyFlags(void (*f)(int f1));

// set data
void (*w_motors_setData)(char* data, int len);
__declspec(dllexport) void reg_w_motors_setData(void (*f)(char* data, int len));

// update motor data
void (*w_motors_updateMotorData)(MOTORDATA* data, int index, UBYTE isReset);
__declspec(dllexport) void reg_w_motors_updateMotorData(void (*f)(MOTORDATA* data, int index, UBYTE isReset));

#endif // W_MOTORS_H_