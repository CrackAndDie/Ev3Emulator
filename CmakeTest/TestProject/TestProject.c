// TestProject.cpp : Defines the entry point for the application.
//

#include "TestProject.h"

static int anime = 42;

void getAbb(int* abb) {
	*abb = sizeof(int*);
	anime = *abb;
}

int getAnime() {
	return anime;
}
