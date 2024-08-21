#ifndef   W_BUTTONS_H_
#define   W_BUTTONS_H_

// get array of (6) buttons states like { 0, 0, 1, 0, 0, 0 }
unsigned char* (*w_buttons_getPressed)(void);
__declspec(dllexport) void reg_w_button_getPressed(unsigned char* (*f)(void));


#endif // W_BUTTONS_H_