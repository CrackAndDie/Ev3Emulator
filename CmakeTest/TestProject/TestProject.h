// TestProject.h : Include file for standard system include files,
// or project specific include files.

#ifndef   TESTPROJECT_H_
#define   TESTPROJECT_H_

__declspec(dllexport) unsigned char* custom_malloc(unsigned int amount, unsigned char fillZeros);
__declspec(dllexport) void custom_free(unsigned char* ptr);

__declspec(dllexport) int anime322(unsigned int point_num_per_ch, short* p_raw_data, unsigned int* p_points_per_ch_returned);

#endif /* TESTPROJECT_H_ */