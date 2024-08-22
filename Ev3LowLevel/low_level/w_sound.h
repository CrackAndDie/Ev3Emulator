#ifndef   W_SOUND_H_
#define   W_SOUND_H_

// play tone
void (*w_sound_playTone)(short freq);
__declspec(dllexport) void reg_w_sound_playTone(void (*f)(short freq));

// is tone playing
int (*w_sound_isTonePlaying)(void);
__declspec(dllexport) void reg_w_sound_isTonePlaying(int (*f)(void));

// play chunk
void (*w_sound_playChunk)(unsigned char* data, int len);
__declspec(dllexport) void reg_w_sound_playChunk(void (*f)(unsigned char* data, int len));

#endif // W_SOUND_H_