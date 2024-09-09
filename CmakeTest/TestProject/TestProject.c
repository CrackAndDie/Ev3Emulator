// TestProject.cpp : Defines the entry point for the application.
//

#include "TestProject.h"
#include <stdlib.h>

unsigned char* custom_malloc(unsigned int amount, unsigned char fillZeros) {
	unsigned char* data;
	data = (unsigned char*)malloc(amount);
	if (fillZeros && data != NULL) {
		for (int i = 0; i < amount; ++i) {
			data[i] = 0;
		}
	}
	return data;
}

void custom_free(unsigned char* ptr) {
	free(ptr);
}

int anime322(unsigned int point_num_per_ch, short* p_raw_data, unsigned int* p_points_per_ch_returned) {
	for (unsigned int i = 0; i < point_num_per_ch; ++i) {
		p_raw_data[i] = 1488;
	}
	*p_points_per_ch_returned = point_num_per_ch;
	return 0;
}
