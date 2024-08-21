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

#ifndef DIALOG_H_
#define DIALOG_H_

#include "lms2012.h"

DATA8 cUiNotification(DATA8 Color, DATA16 X, DATA16 Y, DATA8 Icon1, DATA8 Icon2,
                      DATA8 Icon3, DATA8 *pText, DATA8 *pState);
DATA8 cUiQuestion(DATA8 Color, DATA16 X, DATA16 Y, DATA8 Icon1, DATA8 Icon2,
                  DATA8 *pText, DATA8 *pState, DATA8 *pAnswer);
RESULT cUiIconQuestion(DATA8 Color, DATA16 X, DATA16 Y, DATA8 *pState,
                       DATA32 *pIcons);

#endif /* DIALOG_H_ */
