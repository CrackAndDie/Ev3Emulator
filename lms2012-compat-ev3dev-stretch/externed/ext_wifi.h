#ifndef EXT_WIFI_H_
#define EXT_WIFI_H_

char* ext_ipAddr = "13.37.22.8";
char* ext_macAddr = "13-37-22-8A-E3-22";
char* ext_wifiName = "ev3-rcv";

void ext_getIpAddr(char* addr);
void ext_setIpAddr(const char* addr);

void ext_getMacAddr(char* addr);
void ext_setMacAddr(const char* addr);

void ext_getWifiName(char* name);
void ext_setWifiName(const char* name);

void (*ext_wifiDataFromBrickCallback)(unsigned char*, unsigned short);

void ext_registerWifiDataFromBrickCallback(void (*f)(unsigned char*, unsigned short));

#endif /* EXT_WIFI_H_ */