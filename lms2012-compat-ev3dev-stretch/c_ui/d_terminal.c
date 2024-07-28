/*
 * LEGO® MINDSTORMS EV3
 *
 * Copyright (C) 2010-2013 The LEGO Group
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

#include "lms2012.h"

#include <stdio.h>

static RESULT TerminalResult = FAIL;

RESULT dTerminalInit(void)
{
    RESULT Result = FAIL;
    TerminalResult = Result;

    return Result;
}

// TODO: wtf is terminal
RESULT dTerminalRead(UBYTE *pData)
{
#ifdef  DEBUG_TRACE_KEY
    static  int OldTmp = 1;
#endif
    RESULT  Result = FAIL;
    int     Tmp;

    if (TerminalResult == OK) {
        Result = BUSY;

        /*Tmp = read(STDIN_FILENO, pData, 1);
        if (Tmp == 1) {
            Result = OK;
#ifdef  DEBUG_TRACE_KEY
            printf("[%c]",(char)*pData);
#endif
        }*/
#ifdef  DEBUG_TRACE_KEY
        else {
            if (Tmp != OldTmp) {
                printf("{%d}",Tmp);
            }
        }
        OldTmp = Tmp;
#endif
    }

    return Result;
}

// TODO: wtf is terminal
RESULT dTerminalWrite(UBYTE *pData, UWORD Cnt)
{
    if (TerminalResult == OK) {
        /*if (write(STDOUT_FILENO, pData, Cnt) != Cnt) {
            TerminalResult = FAIL;
        }*/
    }

    return OK;
}

// TODO: wtf is terminal
RESULT dTerminalExit(void)
{
   /* if (TerminalResult == OK) {
        tcsetattr(STDIN_FILENO, TCSAFLUSH, &TerminalSavedAttr);
    }*/
    TerminalResult = FAIL;

    return OK;
}
