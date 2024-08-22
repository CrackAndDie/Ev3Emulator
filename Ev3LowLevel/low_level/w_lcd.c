#include "w_lcd.h"

void reg_w_lcd_updateLcd(void (*f)(unsigned char* buff, int size))
{
    w_lcd_updateLcd = f;
}

void reg_w_lcd_updateLed(void (*f)(int state))
{
    w_lcd_updateLed = f;
}