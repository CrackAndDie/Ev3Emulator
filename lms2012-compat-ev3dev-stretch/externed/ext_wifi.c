#include "ext_wifi.h"

void ext_getIpAddr(char* addr)
{ 
    strcpy(addr, ext_ipAddr);
}
void ext_setIpAddr(const char* addr)
{ 
    strcpy(ext_ipAddr, addr);
}

void ext_getMacAddr(char* addr)
{ 
    strcpy(addr, ext_macAddr);
}
void ext_setMacAddr(const char* addr)
{ 
    strcpy(ext_macAddr, addr);
}

void ext_getWifiName(char* name)
{ 
    strcpy(name, ext_wifiName);
}
void ext_setWifiName(const char* name)
{ 
    strcpy(ext_wifiName, name);
}

void ext_registerWifiDataFromBrickCallback(void (*f)(unsigned char*, unsigned short))
{
    ext_wifiDataFromBrickCallback = f;
}

