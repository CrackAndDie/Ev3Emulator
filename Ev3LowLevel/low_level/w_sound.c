#include "w_sound.h"

void reg_w_sound_playTone(void (*f)(short freq))
{
    w_sound_playTone = f;
}

void reg_w_sound_isTonePlaying(int (*f)(void))
{
    w_sound_isTonePlaying = f;
}

void reg_w_sound_playChunk(void (*f)(unsigned char* data, int len))
{
    w_sound_playChunk = f;
}
