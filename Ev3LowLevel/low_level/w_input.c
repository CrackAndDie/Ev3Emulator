#include "w_input.h"
#include "lms2012.h"

void reg_w_input_ioctlIICDAT(void (*f)(int par, IICDAT* data))
{
    w_input_ioctlIICDAT = f;
}

void reg_w_input_ioctlIICSTR(void (*f)(int par, IICSTR* data))
{
    w_input_ioctlIICSTR = f;
}

void reg_w_input_ioctlIICDEVCON(void (*f)(int par, DEVCON* data))
{
    w_input_ioctlIICDEVCON = f;
}

void reg_w_input_ioctlUARTCTL(void (*f)(int par, UARTCTL* data))
{
    w_input_ioctlUARTCTL = f;
}

void reg_w_input_ioctlUARTDEVCON(void (*f)(int par, DEVCON* data))
{
    w_input_ioctlUARTDEVCON = f;
}

void reg_w_input_updateANALOG(void (*f)(ANALOG* data))
{
    w_input_updateANALOG = f;
}

// ------- writes

void reg_w_input_writeData(void (*f)(int par, unsigned char* data, int len))
{
    w_input_writeData = f;
}