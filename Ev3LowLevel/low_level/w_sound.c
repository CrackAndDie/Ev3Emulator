#include "w_sound.h"

void reg_w_sound_playTone(void (*f)(short freq, unsigned short duration))
{
    w_sound_playTone = f;
}

void reg_w_sound_isSoundPlaying(int (*f)(void))
{
    w_sound_isSoundPlaying = f;
}

void reg_w_sound_playSound(void (*f)(unsigned char* name, int size, int rate, int volume))
{
    w_sound_playSound = f;
}
