#ifndef   W_LCD_H_
#define   W_LCD_H_

// update lcd
void (*w_lcd_updateLcd)(unsigned char* buff, int size);
__declspec(dllexport) void reg_w_lcd_updateLcd(void (*f)(unsigned char* buff, int size));


#endif // W_LCD_H_