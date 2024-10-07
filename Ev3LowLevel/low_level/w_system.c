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

// Function to replace a string with another 
// string 
char* w_system_replaceWord(const char* s, const char* oldW,
	const char* newW)
{
	char* result;
	int i, cnt = 0;
	int newWlen = strlen(newW);
	int oldWlen = strlen(oldW);

	// Counting the number of times old word 
	// occur in the string 
	for (i = 0; s[i] != '\0'; i++) {
		if (strstr(&s[i], oldW) == &s[i]) {
			cnt++;

			// Jumping to index after the old word. 
			i += oldWlen - 1;
		}
	}

	// Making new string of enough length 
	result = (char*)malloc(i + cnt * (newWlen - oldWlen) + 1);

	i = 0;
	while (*s) {
		// compare the substring with the result 
		if (strstr(s, oldW) == s) {
			strcpy(&result[i], newW);
			i += newWlen;
			s += oldWlen;
		}
		else
			result[i++] = *s++;
	}

	result[i] = '\0';
	return result;
}