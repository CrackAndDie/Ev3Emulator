#include "w_buttons.h"

void reg_w_button_getPressed(unsigned char* (*f)(void))
{
    w_buttons_getPressed = f;
}