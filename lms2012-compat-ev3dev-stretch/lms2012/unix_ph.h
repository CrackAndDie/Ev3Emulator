// UNIX placeholder for WINDOWS

#ifndef C_UNIX_PH_H_
#define C_UNIX_PH_H_

void sync(void);

int chmod(const char *path, int mode);
int mkdir_cst(const char *path, int mode);

#endif /* C_UNIX_PH_H_ */
