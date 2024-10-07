#ifndef   W_WIFI_H_
#define   W_WIFI_H_

// start conn
void (*w_wifi_startConnections)(void);
__declspec(dllexport) void reg_w_wifi_startConnections(void (*f)(void));

// stop conn
void (*w_wifi_stopConnections)(void);
__declspec(dllexport) void reg_w_wifi_stopConnections(void (*f)(void));

// read data
int (*w_wifi_readData)(unsigned char * buf, int amount);
__declspec(dllexport) void reg_w_wifi_readData(int (*f)(unsigned char* buf, int amount));

// write data
int (*w_wifi_writeData)(unsigned char* buf, int amount);
__declspec(dllexport) void reg_w_wifi_writeData(int (*f)(unsigned char* buf, int amount));

// stop conn
unsigned char (*w_wifi_isConnected)(void);
__declspec(dllexport) void reg_w_wifi_isConnected(unsigned char (*f)(void));

#endif    // W_WIFI_H_