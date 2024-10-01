#include "w_system.h"
#include "lms2012.h"
#include <stdio.h>
#include <stdarg.h>

#ifdef WIN32
#include <windows.h>
#elif _POSIX_C_SOURCE >= 199309L
#include <time.h>   // for nanosleep
#else
#include <unistd.h> // for usleep
#endif

#define __LOG_FILENAME "./low_level_log.txt"

void w_system_startMain(void) {
	// init shite

	// clear log file
	ptrdiff_t     File;
	File = fopen(__LOG_FILENAME, "w");
	fclose(File);

	w_system_printf("starting main... \n");

	lmsMain(0);
}


void reg_w_system_getStopMain(unsigned char (*f)(void))
{
	w_system_getStopMain = f;
}

int w_system_printf(const char* format, ...) {
	int res = 0;
	va_list argptr;
	va_start(argptr, format);

#ifndef DEBUG_TO_FILE
	res = vprintf(format, argptr);
#else
	ptrdiff_t     File;
	File = fopen(__LOG_FILENAME, "a");
	if (File >= MIN_HANDLE)
	{
		res = vfprintf(File, format, argptr);
		fclose(File);
	}
#endif

	va_end(argptr);
	return res;
}

void w_system_sleep_ms(int milliseconds) {
#ifdef WIN32
	Sleep(milliseconds);
#elif _POSIX_C_SOURCE >= 199309L
	struct timespec ts;
	ts.tv_sec = milliseconds / 1000;
	ts.tv_nsec = (milliseconds % 1000) * 1000000;
	nanosleep(&ts, NULL);
#else
	if (milliseconds >= 1000)
		sleep(milliseconds / 1000);
	usleep((milliseconds % 1000) * 1000);
#endif
}