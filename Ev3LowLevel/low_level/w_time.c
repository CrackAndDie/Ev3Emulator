#include "w_time.h"

void reg_w_time_getMs(unsigned int (*f)(void))
{
    w_time_getMs = f;
}

void reg_w_time_getUs(unsigned int (*f)(void))
{
    w_time_getUs = f;
}
