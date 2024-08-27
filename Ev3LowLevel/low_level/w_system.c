#include "w_system.h"
#include "lms2012.h"
#include <stdio.h>
#include <stdarg.h>

#define __LOG_FILENAME "./low_level_log.txt"

void w_system_startMain(void) {
	// init shite

	// clear log file
	int     File;
	File = fopen(__LOG_FILENAME, "w");
	fclose(File);

	w_system_printf("starting main... \n");

	lmsMain(0);
}

int w_system_printf(const char* format, ...) {
	int res = 0;
	va_list argptr;
	va_start(argptr, format);

#ifndef DEBUG_TO_FILE
	res = vprintf(format, argptr);
#else
	int     File;
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