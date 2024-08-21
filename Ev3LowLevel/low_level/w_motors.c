#include "w_motors.h"

void reg_w_motors_getBusyFlags(void (*f)(int* f1, int* f2))
{
    w_motors_getBusyFlags = f;
}

void reg_w_motors_setBusyFlags(void (*f)(int f1))
{
    w_motors_setBusyFlags = f;
}

void reg_w_motors_setData(void (*f)(char* data, int len))
{
    w_motors_setData = f;
}