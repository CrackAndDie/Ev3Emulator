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

#include "lms2012.h"
#include "button.h"
#include "c_ui.h"

#include <errno.h>
#include <fcntl.h>
#include <stdio.h>
#include <string.h>

#define REAL_UP_BUTTON      0
#define REAL_ENTER_BUTTON   1
#define REAL_DOWN_BUTTON    2
#define REAL_RIGHT_BUTTON   3
#define REAL_LEFT_BUTTON    4
#define REAL_BACK_BUTTON    5
#define REAL_ANY_BUTTON     6
#define REAL_NO_BUTTON      7

static const DATA8 MappedToReal[BUTTONTYPES] = {
	[UP_BUTTON] = REAL_UP_BUTTON,
	[ENTER_BUTTON] = REAL_ENTER_BUTTON,
	[DOWN_BUTTON] = REAL_DOWN_BUTTON,
	[RIGHT_BUTTON] = REAL_RIGHT_BUTTON,
	[LEFT_BUTTON] = REAL_LEFT_BUTTON,
	[BACK_BUTTON] = REAL_BACK_BUTTON,
	[ANY_BUTTON] = REAL_ANY_BUTTON,
	[NO_BUTTON] = REAL_NO_BUTTON,
};

/**
 * @brief Tries to open the evdev character device used for button input.
 *
 * Use udev to search for a Linux input device that will serve as the 6 buttons
 * used on the EV3 platform.
 *
 * @return The file descriptor or -1 on error.
 */
int cUiButtonOpenFile(void)
{
	fprintf(stderr, "Failed to get button input device\n");
	
	return -1;
}

/**
 * @brief Clear the state of all buttons.
 */
void cUiButtonClearAll(void)
{
	DATA8 Button;

	for (Button = 0; Button < BUTTONS; Button++) {
		UiInstance.ButtonState[Button] &= ~BUTTON_STATE_MASK;
	}
}

void cUiUpdateButtons(DATA16 Time)
{
	DATA8   Button;

	// TODO: Ideally, we would be using evdev or ev3devKit to get input events
	// rather than manually polling the key state like we are doing here with
	// the EVIOCGKEY ioctl

	// TODO:!! get button inputs

	for (Button = 0; Button < BUTTONS; Button++) {
		// Check virtual buttons (hardware, direct command, PC)

		if (UiInstance.ButtonState[Button] & BUTTON_STATE_ACTIVE) {
			if (!(UiInstance.ButtonState[Button] & BUTTON_STATE_PRESSED)) {
				// Button activated
				UiInstance.Activated = BUTTON_ACTIVATION_SET;
				UiInstance.ButtonState[Button] |= BUTTON_STATE_PRESSED;
				UiInstance.ButtonState[Button] |= BUTTON_STATE_ACTIVATED;
				UiInstance.ButtonTimer[Button] = 0;
				UiInstance.ButtonRepeatTimer[Button] = BUTTON_START_REPEAT_TIME;
			}

			// Control auto repeat
			if (UiInstance.ButtonRepeatTimer[Button] > Time) {
				UiInstance.ButtonRepeatTimer[Button] -= Time;
			}
			else {
				if ((Button != REAL_ENTER_BUTTON) && (Button != REAL_BACK_BUTTON)) {
					// No repeat on ENTER and BACK
					UiInstance.Activated |= BUTTON_ACTIVATION_SET;
					UiInstance.ButtonState[Button] |= BUTTON_STATE_ACTIVATED;
					UiInstance.ButtonRepeatTimer[Button] = BUTTON_REPEAT_TIME;
				}
			}

			// Control long press
			UiInstance.ButtonTimer[Button] += Time;

			if (UiInstance.ButtonTimer[Button] >= LONG_PRESS_TIME) {
				if (!(UiInstance.ButtonState[Button] & BUTTON_STATE_LONG_LATCH)) {
					// Only once
					UiInstance.ButtonState[Button] |= BUTTON_STATE_LONG_LATCH;
#ifdef BUFPRINTSIZE
					if (Button == 2) {
						UiInstance.Activated |= BUTTON_ACTIVATION_BUFPRINT;
					}
#endif
				}
				UiInstance.ButtonState[Button] |= BUTTON_STATE_LONGPRESS;
			}
		}
		else {
			if ((UiInstance.ButtonState[Button] & BUTTON_STATE_PRESSED)) {
				// Button released
				UiInstance.ButtonState[Button] &= ~BUTTON_STATE_PRESSED;
				UiInstance.ButtonState[Button] &= ~BUTTON_STATE_LONG_LATCH;
				UiInstance.ButtonState[Button] |= BUTTON_STATE_BUMPED;
			}
		}
	}
}

static DATA8 cUiButtonRemap(DATA8 Mapped)
{
	DATA8   Real;

	if ((Mapped >= 0) && (Mapped < BUTTONTYPES))
	{
		Real = MappedToReal[Mapped];
	}
	else
	{
		Real = REAL_ANY_BUTTON;
	}

	return (Real);
}

static void cUiButtonSetPress(DATA8 Button, DATA8 Press)
{
	Button = cUiButtonRemap(Button);

	if (Button < BUTTONS)
	{
		if (Press)
		{
			UiInstance.ButtonState[Button] |= BUTTON_STATE_ACTIVE;
		}
		else
		{
			UiInstance.ButtonState[Button] &= ~BUTTON_STATE_ACTIVE;
		}
	}
	else
	{
		if (Button == REAL_ANY_BUTTON)
		{
			if (Press)
			{
				for (Button = 0; Button < BUTTONS; Button++)
				{
					UiInstance.ButtonState[Button] |= BUTTON_STATE_ACTIVE;
				}
			}
			else
			{
				for (Button = 0; Button < BUTTONS; Button++)
				{
					UiInstance.ButtonState[Button] &= ~BUTTON_STATE_ACTIVE;
				}
			}
		}
	}
}

static DATA8 cUiButtonGetPress(DATA8 Button)
{
	DATA8   Result = 0;

	Button = cUiButtonRemap(Button);

	if (Button < BUTTONS)
	{
		if (UiInstance.ButtonState[Button] & BUTTON_STATE_PRESSED)
		{
			Result = 1;
		}
	}
	else
	{
		if (Button == REAL_ANY_BUTTON)
		{
			for (Button = 0; Button < BUTTONS; Button++)
			{
				if (UiInstance.ButtonState[Button] & BUTTON_STATE_PRESSED)
				{
					Result = 1;
				}
			}
		}
	}

	return (Result);
}

DATA8 cUiButtonTestShortPress(DATA8 Button)
{
	DATA8   Result = 0;

	Button = cUiButtonRemap(Button);

	if (Button < BUTTONS)
	{
		if (UiInstance.ButtonState[Button] & BUTTON_STATE_ACTIVATED)
		{
			Result = 1;
		}
	}
	else
	{
		if (Button == REAL_ANY_BUTTON)
		{
			for (Button = 0; Button < BUTTONS; Button++)
			{
				if (UiInstance.ButtonState[Button] & BUTTON_STATE_ACTIVATED)
				{
					Result = 1;
				}
			}
		}
		else
		{
			if (Button == REAL_NO_BUTTON)
			{
				Result = 1;
				for (Button = 0; Button < BUTTONS; Button++)
				{
					if (UiInstance.ButtonState[Button] & BUTTON_STATE_ACTIVATED)
					{
						Result = 0;
					}
				}
			}
		}
	}

	return (Result);
}

DATA8 cUiButtonGetShortPress(DATA8 Button)
{
	DATA8   Result = 0;

	Button = cUiButtonRemap(Button);

	if (Button < BUTTONS)
	{
		if (UiInstance.ButtonState[Button] & BUTTON_STATE_ACTIVATED)
		{
			UiInstance.ButtonState[Button] &= ~BUTTON_STATE_ACTIVATED;
			Result = 1;
		}
	}
	else
	{
		if (Button == REAL_ANY_BUTTON)
		{
			for (Button = 0; Button < BUTTONS; Button++)
			{
				if (UiInstance.ButtonState[Button] & BUTTON_STATE_ACTIVATED)
				{
					UiInstance.ButtonState[Button] &= ~BUTTON_STATE_ACTIVATED;
					Result = 1;
				}
			}
		}
		else
		{
			if (Button == REAL_NO_BUTTON)
			{
				Result = 1;
				for (Button = 0; Button < BUTTONS; Button++)
				{
					if (UiInstance.ButtonState[Button] & BUTTON_STATE_ACTIVATED)
					{
						UiInstance.ButtonState[Button] &= ~BUTTON_STATE_ACTIVATED;
						Result = 0;
					}
				}
			}
		}
	}
	if (Result)
	{
		UiInstance.Click = 1;
	}

	return (Result);
}

static DATA8 cUiButtonGetBumped(DATA8 Button)
{
	DATA8   Result = 0;

	Button = cUiButtonRemap(Button);

	if (Button < BUTTONS)
	{
		if (UiInstance.ButtonState[Button] & BUTTON_STATE_BUMPED)
		{
			UiInstance.ButtonState[Button] &= ~BUTTON_STATE_BUMPED;
			Result = 1;
		}
	}
	else
	{
		if (Button == REAL_ANY_BUTTON)
		{
			for (Button = 0; Button < BUTTONS; Button++)
			{
				if (UiInstance.ButtonState[Button] & BUTTON_STATE_BUMPED)
				{
					UiInstance.ButtonState[Button] &= ~BUTTON_STATE_BUMPED;
					Result = 1;
				}
			}
		}
		else
		{
			if (Button == REAL_NO_BUTTON)
			{
				Result = 1;
				for (Button = 0; Button < BUTTONS; Button++)
				{
					if (UiInstance.ButtonState[Button] & BUTTON_STATE_BUMPED)
					{
						UiInstance.ButtonState[Button] &= ~BUTTON_STATE_BUMPED;
						Result = 0;
					}
				}
			}
		}
	}

	return (Result);
}

DATA8 cUiButtonTestLongPress(DATA8 Button)
{
	DATA8   Result = 0;

	Button = cUiButtonRemap(Button);

	if (Button < BUTTONS)
	{
		if (UiInstance.ButtonState[Button] & BUTTON_STATE_LONGPRESS)
		{
			Result = 1;
		}
	}
	else
	{
		if (Button == REAL_ANY_BUTTON)
		{
			for (Button = 0; Button < BUTTONS; Button++)
			{
				if (UiInstance.ButtonState[Button] & BUTTON_STATE_LONGPRESS)
				{
					Result = 1;
				}
			}
		}
		else
		{
			if (Button == REAL_NO_BUTTON)
			{
				Result = 1;
				for (Button = 0; Button < BUTTONS; Button++)
				{
					if (UiInstance.ButtonState[Button] & BUTTON_STATE_LONGPRESS)
					{
						Result = 0;
					}
				}
			}
		}
	}

	return (Result);
}

static DATA8 cUiButtonGetLongPress(DATA8 Button)
{
	DATA8   Result = 0;

	Button = cUiButtonRemap(Button);

	if (Button < BUTTONS)
	{
		if (UiInstance.ButtonState[Button] & BUTTON_STATE_LONGPRESS)
		{
			UiInstance.ButtonState[Button] &= ~BUTTON_STATE_LONGPRESS;
			Result = 1;
		}
	}
	else
	{
		if (Button == REAL_ANY_BUTTON)
		{
			for (Button = 0; Button < BUTTONS; Button++)
			{
				if (UiInstance.ButtonState[Button] & BUTTON_STATE_LONGPRESS)
				{
					UiInstance.ButtonState[Button] &= ~BUTTON_STATE_LONGPRESS;
					Result = 1;
				}
			}
		}
		else
		{
			if (Button == REAL_NO_BUTTON)
			{
				Result = 1;
				for (Button = 0; Button < BUTTONS; Button++)
				{
					if (UiInstance.ButtonState[Button] & BUTTON_STATE_LONGPRESS)
					{
						UiInstance.ButtonState[Button] &= ~BUTTON_STATE_LONGPRESS;
						Result = 0;
					}
				}
			}
		}
	}
	if (Result)
	{
		UiInstance.Click = 1;
	}

	return (Result);
}

DATA16 cUiButtonTestHorz(void)
{
	DATA16  Result = 0;

	if (cUiButtonTestShortPress(LEFT_BUTTON))
	{
		Result = -1;
	}
	if (cUiButtonTestShortPress(RIGHT_BUTTON))
	{
		Result = 1;
	}

	return (Result);
}

DATA16 cUiButtonGetHorz(void)
{
	DATA16  Result = 0;

	if (cUiButtonGetShortPress(LEFT_BUTTON))
	{
		Result = -1;
	}
	if (cUiButtonGetShortPress(RIGHT_BUTTON))
	{
		Result = 1;
	}

	return (Result);
}

DATA16 cUiButtonGetVert(void)
{
	DATA16  Result = 0;

	if (cUiButtonGetShortPress(UP_BUTTON))
	{
		Result = -1;
	}
	if (cUiButtonGetShortPress(DOWN_BUTTON))
	{
		Result = 1;
	}

	return (Result);
}

static DATA8 cUiButtonWaitForPress(void)
{
	DATA8   Result = 0;

	Result = cUiButtonTestShortPress(ANY_BUTTON);

	return (Result);
}

/*! \page cUi
 *  <hr size="1"/>
 *  <b>     opUI_BUTTON (CMD, ....)  </b>
 *
 *- UI button\n
 *- Dispatch status unchanged
 *
 *  \param  (DATA8)   CMD           - \ref uibuttonsubcode
 *
 *  - CMD = FLUSH
 *
 *\n
 *
 *  - CMD = WAIT_FOR_PRESS
 *
 *\n
 *
 *  - CMD = PRESSED
 *    -  \param  (DATA8)   BUTTON   - \ref buttons \n
 *    -  \return (DATA8)   STATE    - Button is pressed (0 = no, 1 = yes)\n
 *
 *\n
 *
 *  - CMD = SHORTPRESS
 *    -  \param  (DATA8)   BUTTON   - \ref buttons \n
 *    -  \return (DATA8)   STATE    - Button has been pressed (0 = no, 1 = yes)\n
 *
 *\n
 *  - CMD = GET_BUMPED
 *    -  \param  (DATA8)   BUTTON   - \ref buttons \n
 *    -  \return (DATA8)   STATE    - Button has been pressed (0 = no, 1 = yes)\n
 *
 *\n
 *
 *  - CMD = LONGPRESS
 *    -  \param  (DATA8)   BUTTON   - \ref buttons \n
 *    -  \return (DATA8)   STATE    - Button has been hold down(0 = no, 1 = yes)\n
 *
 *\n
 *  - CMD = PRESS
 *    -  \param  (DATA8)   BUTTON   - \ref buttons \n
 *
 *\n
 *  - CMD = RELEASE
 *    -  \param  (DATA8)   BUTTON   - \ref buttons \n
 *
 *\n
 *  - CMD = GET_HORZ
 *    -  \return (DATA16)  VALUE    - Horizontal arrows data (-1 = left, +1 = right, 0 = not pressed)\n
 *
 *\n
 *  - CMD = GET_VERT
 *    -  \return (DATA16)  VALUE    - Vertical arrows data (-1 = up, +1 = down, 0 = not pressed)\n
 *
 *\n
 *  - CMD = SET_BACK_BLOCK
 *    -  \param  (DATA8)   BLOCKED  - Set UI back button blocked flag (0 = not blocked, 1 = blocked)\n
 *
 *\n
 *  - CMD = GET_BACK_BLOCK
 *    -  \return (DATA8)   BLOCKED  - Get UI back button blocked flag (0 = not blocked, 1 = blocked)\n
 *
 *\n
 *
 *  - CMD = TESTSHORTPRESS
 *    -  \param  (DATA8)   BUTTON   - \ref buttons \n
 *    -  \return (DATA8)   STATE    - Button has been hold down(0 = no, 1 = yes)\n
 *
 *\n
 *
 *  - CMD = TESTLONGPRESS
 *    -  \param  (DATA8)   BUTTON   - \ref buttons \n
 *    -  \return (DATA8)   STATE    - Button has been hold down(0 = no, 1 = yes)\n
 *
 *\n
 *  - CMD = GET_CLICK
 *\n  Get and clear click sound request (internal use only)\n
 *    -  \return (DATA8)   CLICK    - Click sound request (0 = no, 1 = yes)\n
 *
 *\n
 *
 */
 /*! \brief  opUI_BUTTON byte code
  *
  */
void      cUiButton(void)
{
	PRGID   TmpPrgId;
	OBJID   TmpObjId;
	IP      TmpIp;
	DATA8   Cmd;
	DATA8   Button;
	DATA8   State;
	DATA16  Inc;
	DATA8   Blocked;

	TmpIp = GetObjectIp();
	TmpPrgId = CurrentProgramId();

	if (UiInstance.ScreenBlocked == 0)
	{
		Blocked = 0;
	}
	else
	{
		TmpObjId = CallingObjectId();
		if ((TmpPrgId == UiInstance.ScreenPrgId) && (TmpObjId == UiInstance.ScreenObjId))
		{
			Blocked = 0;
		}
		else
		{
			Blocked = 1;
		}
	}

	Cmd = *(DATA8*)PrimParPointer();

	State = 0;
	Inc = 0;

	switch (Cmd)
	{ // Function

	case PRESS:
	{
		Button = *(DATA8*)PrimParPointer();
		cUiButtonSetPress(Button, 1);
	}
	break;

	case RELEASE:
	{
		Button = *(DATA8*)PrimParPointer();
		cUiButtonSetPress(Button, 0);
	}
	break;

	case SHORTPRESS:
	{
		Button = *(DATA8*)PrimParPointer();

		if (Blocked == 0)
		{
			State = cUiButtonGetShortPress(Button);
		}
		*(DATA8*)PrimParPointer() = State;
	}
	break;

	case GET_BUMBED:
	{
		Button = *(DATA8*)PrimParPointer();

		if (Blocked == 0)
		{
			State = cUiButtonGetBumped(Button);
		}
		*(DATA8*)PrimParPointer() = State;
	}
	break;

	case PRESSED:
	{
		Button = *(DATA8*)PrimParPointer();

		if (Blocked == 0)
		{
			State = cUiButtonGetPress(Button);
		}
		*(DATA8*)PrimParPointer() = State;
	}
	break;

	case LONGPRESS:
	{
		Button = *(DATA8*)PrimParPointer();

		if (Blocked == 0)
		{
			State = cUiButtonGetLongPress(Button);
		}
		*(DATA8*)PrimParPointer() = State;
	}
	break;

	case FLUSH:
	{
		if (Blocked == 0)
		{
			cUiButtonClearAll();
		}
	}
	break;

	case WAIT_FOR_PRESS:
	{
		if (Blocked == 0)
		{
			if (cUiButtonWaitForPress() == 0)
			{
				SetObjectIp(TmpIp - 1);
				SetDispatchStatus(BUSYBREAK);
			}
		}
		else
		{
			SetObjectIp(TmpIp - 1);
			SetDispatchStatus(BUSYBREAK);
		}
	}
	break;

	case GET_HORZ:
	{
		if (Blocked == 0)
		{
			Inc = cUiButtonGetHorz();
		}
		*(DATA16*)PrimParPointer() = Inc;
	}
	break;

	case GET_VERT:
	{
		if (Blocked == 0)
		{
			Inc = cUiButtonGetVert();
		}
		*(DATA16*)PrimParPointer() = Inc;
	}
	break;

	case SET_BACK_BLOCK:
	{
		UiInstance.BackButtonBlocked = *(DATA8*)PrimParPointer();
	}
	break;

	case GET_BACK_BLOCK:
	{
		*(DATA8*)PrimParPointer() = UiInstance.BackButtonBlocked;
	}
	break;

	case TESTSHORTPRESS:
	{
		Button = *(DATA8*)PrimParPointer();

		if (Blocked == 0)
		{
			State = cUiButtonTestShortPress(Button);
		}
		*(DATA8*)PrimParPointer() = State;
	}
	break;

	case TESTLONGPRESS:
	{
		Button = *(DATA8*)PrimParPointer();

		if (Blocked == 0)
		{
			State = cUiButtonTestLongPress(Button);
		}
		*(DATA8*)PrimParPointer() = State;
	}
	break;

	case GET_CLICK:
	{
		*(DATA8*)PrimParPointer() = UiInstance.Click;
		UiInstance.Click = 0;
	}
	break;

	}
}
