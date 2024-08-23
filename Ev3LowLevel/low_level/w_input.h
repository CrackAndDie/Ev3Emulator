#ifndef   W_INPUT_H_
#define   W_INPUT_H_

#include "lms2012.h"

// ioctl IICDAT
void (*w_input_ioctlIICDAT)(int par, IICDAT* data);
__declspec(dllexport) void reg_w_input_ioctlIICDAT(void (*f)(int par, IICDAT* data));

// ioctl IICSTR
void (*w_input_ioctlIICSTR)(int par, IICSTR* data);
__declspec(dllexport) void reg_w_input_ioctlIICSTR(void (*f)(int par, IICSTR* data));

// ioctl IICSTR
void (*w_input_ioctlIICDEVCON)(int par, DEVCON* data);
__declspec(dllexport) void reg_w_input_ioctlIICDEVCON(void (*f)(int par, DEVCON* data));

// ioctl UARTCTL
void (*w_input_ioctlUARTCTL)(int par, UARTCTL* data);
__declspec(dllexport) void reg_w_input_ioctlUARTCTL(void (*f)(int par, UARTCTL* data));

// ioctl UARTDEVCON
void (*w_input_ioctlUARTDEVCON)(int par, DEVCON* data);
__declspec(dllexport) void reg_w_input_ioctlUARTDEVCON(void (*f)(int par, DEVCON* data));

// ------- writes

// write to uart/adc/dcm  -  0, 1, 2
void (*w_input_writeData)(int par, char* data, int len);
__declspec(dllexport) void reg_w_input_writeData(void (*f)(int par, char* data, int len));

#endif // W_INPUT_H_