/*
 * LEGO® MINDSTORMS EV3
 *
 * Copyright (C) 2010-2013 The LEGO Group
 * Copyright (C) 2016 David Lechner <david@lechnology.com>
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

/*! \page SoundLibrary Sound Library
 *
 *- \subpage  SoundLibraryDescription
 *- \subpage  SoundLibraryCodes
 */

/*! \page SoundLibraryDescription Description
 *
 *
 */

/*! \page SoundLibraryCodes Byte Code Summary
 *
 *
 */

#include "lms2012.h"
#include "c_sound.h"

#include <errno.h>
#include <fcntl.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>

#ifdef    DEBUG_C_SOUND
#define   DEBUG
#endif

#define DEFAULT_SOUND_CARD          "default"
#define SOUND_FILE_BUFFER_SIZE      128

#define SCALE_VOLUME(vol,min,max)   ((vol) * ((max) - (min)) / 100 + (min))

typedef enum {
    SOUND_STOPPED,
    SOUND_FILE_PLAYING,
    SOUND_FILE_LOOPING,
    SOUND_TONE_PLAYING,
} SOUND_STATES;

typedef struct {
    bool no_sound;
    int hSoundFile;
    int event_fd;
    long save_pcm_volume;
    long save_tone_volume;

    DATA8 SoundOwner;
    SOUND_STATES cSoundState;
    ULONG ToneStartTime;
    UWORD ToneDuration;
    UWORD SoundFileFormat;
    UWORD SoundDataLength;
    UWORD SoundSampleRate;
    UWORD SoundPlayMode;
    SWORD ValPrev;
    SWORD Index;
    SWORD Step;
    size_t BytesToWrite;
    char PathBuffer[MAX_FILENAME_SIZE];
    UBYTE SoundData[SOUND_FILE_BUFFER_SIZE];
} SOUND_GLOBALS;

SOUND_GLOBALS SoundInstance;

/*
 * cSoundPlayTone:
 *
 * Plays a tone using the Linux input event device.
 *
 * Specifying a frequency of 0 stops tone
 *
 * returns -1 if there was an error
 */
static int cSoundPlayTone(DATA16 frequency)
{
    // TOPO: anime228

    /*struct input_event event = {
        .time   = { 0 },
        .type   = EV_SND,
        .code   = SND_TONE,
        .value  = frequency,
    };

    return write(SoundInstance.event_fd, &event, sizeof(event));*/
    return 0;
}

/*
 * cSoundIsTonePlaying:
 *
 * returns a non-zero value if a tone is playing
 */
static int cSoundIsTonePlaying(void)
{
    unsigned char status = 0;

    ioctl(SoundInstance.event_fd, EVIOCGSND(sizeof(status)), &status);

    return status;
}

RESULT cSoundInit(void)
{
    SoundInstance.event_fd = 0;
    SoundInstance.hSoundFile = -1;

    return OK;
}

RESULT cSoundOpen(void)
{
    return OK;
}

RESULT cSoundClose(void)
{
    SoundInstance.cSoundState = SOUND_STOPPED;

    cSoundPlayTone(0);

    if (SoundInstance.hSoundFile >= 0) {
        close(SoundInstance.hSoundFile);
        SoundInstance.hSoundFile = -1;
    }

    return OK;
}

RESULT cSoundUpdate(void)
{
    int     BytesRead;
    UWORD   BytesToRead;
    // UBYTE   BytesWritten = 0;
    RESULT  Result = FAIL;

    switch(SoundInstance.cSoundState) {
    case SOUND_STOPPED:
        // Do nothing
        break;

    case SOUND_FILE_LOOPING:
    case SOUND_FILE_PLAYING:
        //if (SoundInstance.BytesToWrite > 0) {
        //    if (SoundInstance.BytesToWrite > SOUND_FILE_BUFFER_SIZE) {
        //        BytesToRead = SOUND_FILE_BUFFER_SIZE;
        //    } else {
        //        BytesToRead = SoundInstance.BytesToWrite;
        //    }

        //    if (SoundInstance.hSoundFile >= 0) {
        //        int frames = 0;

        //        if (frames <= BytesToRead) {
        //            // if there not enough room, wait for the next loop
        //            Result = OK;
        //            break;
        //        }

        //        BytesRead = read(SoundInstance.hSoundFile,
        //                         SoundInstance.SoundData, BytesToRead);

        //        if (frames < 0) {
        //            fprintf(stderr, "Failed to write sound data to PCM: %s\n",
        //                    snd_strerror(frames));
        //            cSoundClose();
        //            // Result != OK
        //            break;
        //        } else {
        //            SoundInstance.BytesToWrite -= BytesRead;
        //        }
        //    }
        //} else {
        //    // We have finished writing the file, so if we are looping, start
        //    // over, otherwise wait for the sound to finish playing.

        //    if (SoundInstance.cSoundState == SOUND_FILE_LOOPING) {
        //        lseek(SoundInstance.hSoundFile, 8, SEEK_SET);
        //        SoundInstance.BytesToWrite = SoundInstance.SoundDataLength;
        //    } else {
        //        snd_pcm_state_t state = snd_pcm_state(SoundInstance.pcm);

        //        if (state != SND_PCM_STATE_RUNNING) {
        //            cSoundClose();
        //        }
        //    }
        //}

        // TODO: anime228

        Result = OK;

        break;

    case SOUND_TONE_PLAYING:
        if (cSoundIsTonePlaying()) {
            ULONG elapsed = VMInstance.NewTime - SoundInstance.ToneStartTime;

            if (elapsed >= SoundInstance.ToneDuration) {
                // stop the tone
                cSoundPlayTone(0);
            } else {
                // keep playing
                break;
            }
        }
        SoundInstance.cSoundState = SOUND_STOPPED;
        Result = OK;

        break;
    }

    return Result;
}

RESULT cSoundExit(void)
{
    // restore the system volume levels

    return OK;
}

//******* BYTE CODE SNIPPETS **************************************************

/*! \page cSound Sound
 *  <hr size="1"/>
 *  <b>     opSOUND ()  </b>
 *
 *- Memory file entry\n
 *- Dispatch status unchanged
 *
 *  \param  (DATA8)   CMD     - Specific command \ref soundsubcode
 *
 *  - CMD = BREAK\n
 *
 *\n
 *  - CMD = TONE
 *    -  \param  (DATA8)    VOLUME    - Volume [0..100]\n
 *    -  \param  (DATA16)   FREQUENCY - Frequency [Hz]\n
 *    -  \param  (DATA16)   DURATION  - Duration [mS]\n
 *
 *\n
 *  - CMD = PLAY
 *    -  \param  (DATA8)    VOLUME    - Volume [0..100]\n
 *    -  \param  (DATA8)    NAME      - First character in filename (character string)\n
 *
 *\n
 *  - CMD = REPEAT
 *    -  \param  (DATA8)    VOLUME    - Volume [0..100]\n
 *    -  \param  (DATA8)    NAME      - First character in filename (character string)\n
 *
 *\n
 *
 */
/*! \brief  opSOUND byte code
 *
 */
void cSoundEntry(void)
{
    int     Cmd;
    UWORD   Volume;
    UWORD   Frequency;
    UWORD   Duration;
    DATA8   *pFileName;
    char    PathName[MAX_FILENAME_SIZE];
    UBYTE   Tmp1;
    UBYTE   Tmp2;
    UBYTE   Loop = FALSE;

    Cmd = *(DATA8*)PrimParPointer();

    switch(Cmd) {
    case TONE:
        cSoundClose();

        SoundInstance.SoundOwner = CallingObjectId();

        Volume = *(DATA8*)PrimParPointer();
        Frequency = *(DATA16*)PrimParPointer();
        Duration = *(DATA16*)PrimParPointer();

        SoundInstance.ToneStartTime = VMInstance.NewTime;
        SoundInstance.ToneDuration = Duration;
        if (cSoundPlayTone(Frequency) != -1) {
            SoundInstance.cSoundState = SOUND_TONE_PLAYING;
        }

        break;

    case BREAK:
        cSoundClose();

        break;

    case REPEAT:
        Loop = TRUE;
        // Fall through
    case PLAY:
        cSoundClose();

        SoundInstance.SoundOwner = CallingObjectId();

        Volume = *(DATA8*)PrimParPointer();
        pFileName = (DATA8*)PrimParPointer();

        if (pFileName != NULL) {
            // Get Path and concatenate

            PathName[0] = 0;
            if (pFileName[0] != '.') {
                GetResourcePath(PathName, MAX_FILENAME_SIZE);
                sprintf(SoundInstance.PathBuffer, "%s%s.rsf", (char*)PathName,
                        (char*)pFileName);
            } else {
                sprintf(SoundInstance.PathBuffer, "%s.rsf", (char*)pFileName);
            }

            // Open SoundFile

            // TODO: anime228
            SoundInstance.hSoundFile = open(SoundInstance.PathBuffer, O_RDONLY);

            if (SoundInstance.hSoundFile >= 0) {
                // BIG Endianess

                read(SoundInstance.hSoundFile, &Tmp1, 1);
                read(SoundInstance.hSoundFile, &Tmp2, 1);
                SoundInstance.SoundFileFormat = (UWORD)Tmp1 << 8 | (UWORD)Tmp2;

                read(SoundInstance.hSoundFile, &Tmp1, 1);
                read(SoundInstance.hSoundFile, &Tmp2, 1);
                SoundInstance.SoundDataLength = (UWORD)Tmp1 << 8 | (UWORD)Tmp2;

                read(SoundInstance.hSoundFile, &Tmp1, 1);
                read(SoundInstance.hSoundFile, &Tmp2, 1);
                SoundInstance.SoundSampleRate = (UWORD)Tmp1 << 8 | (UWORD)Tmp2;

                read(SoundInstance.hSoundFile, &Tmp1, 1);
                read(SoundInstance.hSoundFile, &Tmp2, 1);
                SoundInstance.SoundPlayMode = (UWORD)Tmp1 << 8 | (UWORD)Tmp2;

                //SoundInstance.pcm = cSoundGetPcm();
                //if (SoundInstance.pcm) {
                //    SoundInstance.cSoundState = Loop ? SOUND_FILE_LOOPING
                //                                     : SOUND_FILE_PLAYING;
                //    SoundInstance.BytesToWrite = SoundInstance.SoundDataLength;
                //}

                // TODO: anime228
            } else {
                fprintf(stderr, "Failed to open sound file '%s': %s\n",
                        SoundInstance.PathBuffer, strerror(errno));
            }
        } else {
            fprintf(stderr, "Sound file name was NULL\n");
        }
        break;
    }
}

/*! \page   cSound
 *  <hr size="1"/>
 *  <b>     opSOUND_TEST (BUSY) </b>
 *
 *- Test if sound busy (playing file or tone\n
 *- Dispatch status unchanged
 *
 *  \return  (DATA8)   BUSY    - Sound busy flag (0 = ready, 1 = busy)
 *
 */
/*! \brief  opSOUND_TEST byte code
 *
 */

void cSoundTest(void)
{
    DATA8 busy = 0;

    if (cSoundIsTonePlaying()) {
        busy = 1;
    }

    *(DATA8*)PrimParPointer() = busy;
}

/*! \page   cSound
 *  <hr size="1"/>
 *  <b>     opSOUND_READY () </b>
 *
 *- Wait for sound ready (wait until sound finished)\n
 *- Dispatch status can change to BUSYBREAK
 *
 */
/*! \brief  opSOUND_READY byte code
 *
 */

void cSoundReady(void)
{
    IP      TmpIp;
    DSPSTAT DspStat = NOBREAK;

    TmpIp = GetObjectIp();

    if (cSoundIsTonePlaying()) {
        // Rewind IP and set status
        DspStat = BUSYBREAK; // break the interpreter and waits busy
        SetDispatchStatus(DspStat);
        SetObjectIp(TmpIp - 1);
    }
}
