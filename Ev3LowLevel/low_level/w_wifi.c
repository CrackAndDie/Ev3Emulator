#include "w_wifi.h"

void reg_w_wifi_startConnections(void (*f)(void))
{
    w_wifi_startConnections = f;
}

void reg_w_wifi_stopConnections(void (*f)(void))
{
    w_wifi_stopConnections = f;
}

void reg_w_wifi_readData(int (*f)(unsigned char* buf, int amount))
{
    w_wifi_readData = f;
}

void reg_w_wifi_writeData(int (*f)(unsigned char* buf, int amount))
{
    w_wifi_writeData = f;
}

void reg_w_wifi_isConnected(unsigned char (*f)(void))
{
    w_wifi_isConnected = f;
}

void reg_w_wifi_isDataAvailable(unsigned char (*f)(void))
{
    w_wifi_isDataAvailable = f;
}