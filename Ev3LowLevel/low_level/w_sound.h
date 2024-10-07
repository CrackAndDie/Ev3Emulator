#ifndef   W_SOUND_H_
#define   W_SOUND_H_

// play tone
void (*w_sound_playTone)(short freq, unsigned short duration);
__declspec(dllexport) void reg_w_sound_playTone(void (*f)(short freq, unsigned short duration));

// is tone playing
int (*w_sound_isSoundPlaying)(void);
__declspec(dllexport) void reg_w_sound_isSoundPlaying(int (*f)(void));

// play chunk
void (*w_sound_playSound)(unsigned char* name, int size, int rate, int volume);
__declspec(dllexport) void reg_w_sound_playSound(void (*f)(unsigned char* name, int size, int rate, int volume));

#endif // W_SOUND_H_