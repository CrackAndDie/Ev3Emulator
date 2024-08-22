#ifndef   W_LCD_H_
#define   W_LCD_H_

// update lcd
void (*w_lcd_updateLcd)(unsigned char* buff, int size);
__declspec(dllexport) void reg_w_lcd_updateLcd(void (*f)(unsigned char* buff, int size));

// update led
void (*w_lcd_updateLed)(int state);
__declspec(dllexport) void reg_w_lcd_updateLed(void (*f)(int state));

#endif // W_LCD_H_