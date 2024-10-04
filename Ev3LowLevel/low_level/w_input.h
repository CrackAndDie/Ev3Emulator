#ifndef   W_INPUT_H_
#define   W_INPUT_H_

#include "lms2012.h"

// update ANALOG
void (*w_input_updateANALOG)(ANALOG* data);
__declspec(dllexport) void reg_w_input_updateANALOG(void (*f)(ANALOG* data));

// update UART
void (*w_input_updateUART)(float* data, int port, int index, int mode);
__declspec(dllexport) void reg_w_input_updateUART(void (*f)(float* data, int port, int index, int mode));

#endif // W_INPUT_H_