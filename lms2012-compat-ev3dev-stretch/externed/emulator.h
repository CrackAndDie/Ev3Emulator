#ifndef EXT_EMULATOR_H_
#define EXT_EMULATOR_H_

// WIFI 
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

int (*ext_wifiDataToBrickCallback)(unsigned char*, unsigned short);
void ext_registerWifiDataToBrickCallback(int (*f)(unsigned char*, unsigned short));

void (*ext_closeTcpFromBrickCallback)(void);
void ext_registerCloseTcpFromBrickCallback(void (*f)(void));

void (*ext_startTcpFromBrickCallback)(void);
void ext_registerStartTcpFromBrickCallback(void (*f)(void));


// end WIFI 

#endif /* EXT_EMULATOR_H_ */