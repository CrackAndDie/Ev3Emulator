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
#include "c_ui.h"
#include "led.h"

#include <errno.h>
#include <fcntl.h>
#include <stdio.h>
#include <string.h>

#define LED_TRIGGER_ON      "default-on"
#define LED_TRIGGER_OFF     "none"
#define LED_TRIGGER_FLASH   "heartbeat"
#define LED_TRIGGER_PULSE   "heartbeat"
// TODO: need a different trigger for flash/pulse

// TODO: this #define and struct is in a kernel header starting with v4.10,
// but we need to run on 4.9 for now.
 #define LED_MAX_NAME_SIZE 64

struct uleds_user_dev {
    char name[LED_MAX_NAME_SIZE];
    int max_brightness;
};

static cUiLedFlags led_state;

/**
 * @brief Tries to open the trigger attribute of a Linux leds subsystem device
 *
 * Use udev to search for a Linux leds class device that will serve as one of
 * the 4 LEDs used on the EV3 platform.
 *
 * @param name  The name of the leds class device
 * @return      The file descriptor or -1 on error.
 */
int cUiLedOpenTriggerFile(const char *name)
{
    int fd = -1;

    return fd;
}

typedef struct {
    int fd;
    cUiLedFlags flags;
} UserLed;


/**
 * @brief Set the state of the brick status LEDs.
 *
 * If UiInstance.Warnlight is active, the color will be set to orange regardless
 * of the state selected.
 *
 * @param State     The new state.
 */
void cUiLedSetState(LEDPATTERN State)
{
    const char *green_trigger;
    const char *red_trigger;

    UiInstance.LedState = State;

    switch (State) {
    case LED_BLACK:
        green_trigger = UiInstance.Warnlight ? LED_TRIGGER_ON : LED_TRIGGER_OFF;
        red_trigger = UiInstance.Warnlight ? LED_TRIGGER_ON : LED_TRIGGER_OFF;
        break;
    case LED_GREEN:
        green_trigger = LED_TRIGGER_ON;
        red_trigger = UiInstance.Warnlight ? LED_TRIGGER_ON : LED_TRIGGER_OFF;
        break;
    case LED_RED:
        green_trigger = UiInstance.Warnlight ? LED_TRIGGER_ON : LED_TRIGGER_OFF;
        red_trigger = LED_TRIGGER_ON;
        break;
    case LED_ORANGE:
        green_trigger = LED_TRIGGER_ON;
        red_trigger = LED_TRIGGER_ON;
        break;
    case LED_GREEN_FLASH:
        green_trigger = LED_TRIGGER_FLASH;
        red_trigger = UiInstance.Warnlight ? LED_TRIGGER_FLASH : LED_TRIGGER_OFF;
        break;
    case LED_RED_FLASH:
        green_trigger = UiInstance.Warnlight ? LED_TRIGGER_FLASH : LED_TRIGGER_OFF;
        red_trigger = LED_TRIGGER_FLASH;
        break;
    case LED_ORANGE_FLASH:
        green_trigger = LED_TRIGGER_FLASH;
        red_trigger = LED_TRIGGER_FLASH;
        break;
    case LED_GREEN_PULSE:
        green_trigger = LED_TRIGGER_PULSE;
        red_trigger = UiInstance.Warnlight ? LED_TRIGGER_PULSE : LED_TRIGGER_OFF;
        break;
    case LED_RED_PULSE:
        green_trigger = UiInstance.Warnlight ? LED_TRIGGER_PULSE : LED_TRIGGER_OFF;
        red_trigger = LED_TRIGGER_PULSE;
        break;
    case LED_ORANGE_PULSE:
        green_trigger = LED_TRIGGER_PULSE;
        red_trigger = LED_TRIGGER_PULSE;
        break;
    default:
        // don't crash if we get bad State
        return;
    }

    // TODO: redirect
}
