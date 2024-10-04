#include "w_input.h"
#include "lms2012.h"

void reg_w_input_updateANALOG(void (*f)(ANALOG* data))
{
    w_input_updateANALOG = f;
}

void reg_w_input_updateUART(void (*f)(float* data, int port, int index, int mode))
{
    w_input_updateUART = f;
}
