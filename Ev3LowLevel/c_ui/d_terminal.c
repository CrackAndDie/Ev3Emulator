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

#include "w_system.h"
#include "lms2012.h"

#include <stdio.h>

//static struct termios TerminalAttr;
//static struct termios TerminalSavedAttr;
static RESULT TerminalResult = FAIL;


RESULT dTerminalInit(void)
{
    RESULT Result = FAIL;

    // TODO: terminal shite
    /*if (tcgetattr(STDIN_FILENO, &TerminalAttr) >= 0) {
        TerminalSavedAttr = TerminalAttr;

        TerminalAttr.c_lflag     &= ~(ECHO | ICANON | IEXTEN);
        TerminalAttr.c_lflag     |= ECHO;
        TerminalAttr.c_iflag     &= ~(BRKINT | ICRNL | INPCK | ISTRIP | IXON);
        TerminalAttr.c_cflag     &= ~(CSIZE | PARENB);
        TerminalAttr.c_cflag     |=  CS8;

        TerminalAttr.c_cc[VMIN]   = 0;
        TerminalAttr.c_cc[VTIME]  = 0;

        if (tcsetattr(STDIN_FILENO, TCSANOW, &TerminalAttr) >= 0) {
            Result = OK;
        }
    }*/
    Result = OK;
    TerminalResult = Result;

    return Result;
}

RESULT dTerminalRead(UBYTE *pData)
{
#ifdef  DEBUG_TRACE_KEY
    static  int OldTmp = 1;
#endif
    RESULT  Result = FAIL;
    int     Tmp;

    // TODO: terminal shite
//    if (TerminalResult == OK) {
//        Result = BUSY;
//
//        Tmp = read(STDIN_FILENO, pData, 1);
//        if (Tmp == 1) {
//            Result = OK;
//#ifdef  DEBUG_TRACE_KEY
//            w_system_printf("[%c]",(char)*pData);
//#endif
//        }
//#ifdef  DEBUG_TRACE_KEY
//        else {
//            if (Tmp != OldTmp) {
//                w_system_printf("{%d}",Tmp);
//            }
//        }
//        OldTmp = Tmp;
//#endif
//    }
    Result = BUSY;

    return Result;
}

RESULT dTerminalWrite(UBYTE *pData, UWORD Cnt)
{
    // TODO: terminal shite
    /*if (TerminalResult == OK) {
        if (write(STDOUT_FILENO, pData, Cnt) != Cnt) {
            TerminalResult = FAIL;
        }
    }*/

    return OK;
}

RESULT dTerminalExit(void)
{
    // TODO: terminal shite
    /*if (TerminalResult == OK) {
        tcsetattr(STDIN_FILENO, TCSAFLUSH, &TerminalSavedAttr);
    }*/
    TerminalResult = FAIL;

    return OK;
}
