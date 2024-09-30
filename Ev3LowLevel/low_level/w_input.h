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

// update ANALOG
void (*w_input_updateANALOG)(ANALOG* data);
__declspec(dllexport) void reg_w_input_updateANALOG(void (*f)(ANALOG* data));

// update UART
void (*w_input_updateUART)(float* data, int port, int index, int mode);
__declspec(dllexport) void reg_w_input_updateUART(void (*f)(float* data, int port, int index, int mode));

// ------- writes

// write to uart/adc/dcm  -  0, 1, 2
void (*w_input_writeData)(int par, unsigned char* data, int len);
__declspec(dllexport) void reg_w_input_writeData(void (*f)(int par, unsigned char* data, int len));

#endif // W_INPUT_H_