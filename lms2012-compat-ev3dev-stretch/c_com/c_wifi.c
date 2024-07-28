/*
 * LEGO® MINDSTORMS EV3
 *
 * Copyright (C) 2010-2013 The LEGO Group
 * Copyright (C) 2016 David Lechner <david@lechnolgy.com>
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

/*! \page WiFiLibrary WiFi Library
 *
 *- \subpage  WiFiLibraryDescription
 *- \subpage  WiFiLibraryCodes
 */


/*! \page WiFiLibraryDescription Description
 *
 *
 */


/*! \page WiFiLibraryCodes Byte Code Summary
 *
 *
 */

#include "c_wifi.h"
#include "emulator.h"

#ifdef DEBUG_WIFI
#define pr_dbg(f, ...) printf(f, ##__VA_ARGS__)
#else
#define pr_dbg(f, ...) while (0) { }
#endif

// States the TCP connection can be in (READ)
typedef enum {
    TCP_IDLE                = 0x00,
    TCP_WAIT_ON_START       = 0x01,
    TCP_WAIT_ON_LENGTH      = 0x02,
    TCP_WAIT_ON_FIRST_CHUNK = 0x04,
    TCP_WAIT_ON_ONLY_CHUNK  = 0x08,
    TCP_WAIT_COLLECT_BYTES  = 0x10
} TCP_READ_STATE;

static char BtSerialNo[13];              // Storage for the BlueTooth Serial Number
static char BrickName[NAME_LENGTH + 1];  // BrickName for discovery and/or friendly info

static RESULT WiFiStatus = OK;

static UWORD TcpTotalLength = 0;
static UWORD TcpRestLen = 0;
static TCP_READ_STATE TcpReadState = TCP_IDLE;
static UWORD TcpReadBufPointer = 0;

/**
 * @brief           Move a service up in the service list.
 *
 * @param Index     The index of the service to move.
 */
void cWiFiMoveUpInList(int Index)
{
}

/**
 * @brief           Move a service down in the service list.
 *
 * @param Index     The index of the service to move.
 */
void cWiFiMoveDownInList(int Index)
{
}

void cWiFiSetEncryptToWpa2(int Index)
{
    WiFiStatus = OK;
    // TODO: Connman does not allow us to select the security.
}

void cWiFiSetEncryptToNone(int Index)
{
    WiFiStatus = OK;
    // TODO: Connman does not allow us to select the security.
}

/**
 * @brief           Get the encryption type for the specified connection.
 *
 * @param Index     The index of the service int the service list.
 *
 * @return          The encryption type.
 */
ENCRYPT cWifiGetEncrypt(int Index)
{
    return ENCRYPT_NONE;
}

/**
 * @brief           Remove the specified service from the list of known services
 *
 * @param Index     The index of the service in the service list.
 */
void cWiFiDeleteAsKnown(int Index)
{
}

/**
 * @brief               Get the IP address of the first service.
 *
 * If there are no services, IpAddress is set to "??".
 *
 * @param IpAddress     Preallocated character array.
 * @return              OK if there is a valid IP address.
 */
RESULT cWiFiGetIpAddr(char* IpAddress)
{
    RESULT Result = OK;

    ext_getIpAddr(IpAddress);

    return Result;
}

/**
 * @brief               Get the MAC address of the first service.
 *
 * If there are no services, MacAddress is set to "??".
 *
 * @param MacAddress    Preallocated character array.
 * @return              OK if there is a valid MAC address or FAIL if there is
 *                      no connection.
 */
RESULT cWiFiGetMyMacAddr(char* MacAddress)
{
    RESULT Result = OK;

    ext_getMacAddr(MacAddress);

    return Result;
}

/**
 * @brief Checks if WiFi is present on the system
 *
 * Also returns OK if ethernet (wired) technology is present.
 *
 * @return OK if present, otherwise FAIL
 */
RESULT cWiFiTechnologyPresent(void)
{
    return OK;
}

/**
 * @brief           Get the name of a service (WiFi AP).
 *
 * Sets ApName to "None" if there is not a service at that index.
 *
 * @param ApName    Preallocated char array to store the name.
 * @param Index     The index of the service.
 * @param Length    The length of ApName.
 * @return          OK if there was a service at the specified index.
 */
RESULT cWiFiGetName(char *ApName, int Index, char Length)
{
    RESULT Result = FAIL;

    Result = OK;
    strncpy(ApName, "None", Length);

    return Result;
}

RESULT cWiFiSetName(char *ApName, int Index)
{
    RESULT Result = FAIL;

    // TODO: This needs to be made to work with ConnMan agent.

    return Result;
}

/**
 * @brief           Gets flags about service properties.
 *
 * @param Index     The index in the service list to check.
 * @return          The flags.
 */
WIFI_STATE_FLAGS cWiFiGetFlags(int Index)
{
    WIFI_STATE_FLAGS flags = VISIBLE; // ConnMan does list services that are not visible

    return flags;
}

/**
 * @brief           Connect to the service at the specified index.
 *
 * @param Index     Index of the service in the service_list.
 *
 * @return          OK on success, BUSY if waiting for completion or FAIL on
 *                  error.
 */
RESULT cWiFiConnectToAp(int Index)
{
    RESULT Result;

    pr_dbg("cWiFiConnectToAp(int Index = %d)\n", Index);

    WiFiStatus = OK;
    Result = OK;

    return Result;
}

// Make the pre-shared key from
// Supplied SSID and PassPhrase
// And store it in ApTable[Index]
RESULT cWiFiMakePsk(char *ApSsid, char *PassPhrase, int Index)
{
    RESULT Result = OK;

    WiFiStatus = OK;

    return Result;
}

/**
 * @brief           Gets the index of a service for a given name
 *
 * @param Name      The name to search for.
 * @param Index     Pointer to store the index if found.
 * @return          OK if a match was found, otherwise FAIL
 */
RESULT cWiFiGetIndexFromName(const char *Name, UBYTE *Index)
{
    RESULT Result = FAIL;

    *Index = 0;

    return Result;
}

/**
 * @brief           Scan for new access points
 *
 * If WiFi is not present or turned off, this does nothing (returns FAIL).
 * This just triggers a scan in the background and returns immediately.
 *
 * @return          OK if scan was started, otherwise FAIL.
 */
RESULT cWiFiScanForAPs()
{
    RESULT Result = FAIL;

    pr_dbg("cWiFiScanForAPs\n");

    Result = OK;

    WiFiStatus = OK;

    return Result;
}

/**
 * @brief           Gets a value that changes each time the service list changes.
 *
 * @return          The value.
 */
DATA16 cWifiGetListState(void)
{
    return 0;
}

/**
 * @brief           Get the size of the service list.
 *
 * @return          The size.
 */
int cWiFiGetApListSize(void)
{
    return 0;
}

RESULT cWiFiGetStatus(void)
{
    pr_dbg("WiFiStatus => GetResult = %d\n", WiFiStatus);

    return WiFiStatus;
}

/**
 * @brief               Write data to a TCP socket.
 *
 * This is a non-blocking operation, so bytes may not be written if the socket
 * is busy. Also returns 0 if there is no active connection.
 *
 * @param Buffer        The data to write.
 * @param Length        The number of bytes to write.
 * @return              The number of bytes actually written.
 */
UWORD cWiFiWriteTcp(UBYTE* Buffer, UWORD Length)
{
    if (Length > 0) {
#if 0 // this makes lots of noise
        printf("\ncWiFiWriteTcp Length: %d\n", Length);
        // Code below used for "hunting" packets of correct length
        // but with length bytes set to "0000" and payload all zeroed

        if ((Buffer[0] == 0) && (Buffer[1] == 0)) {
            int i;
            printf("\ncERROR in first 2 entries - WiFiWriteTcp Length: %d\n", Length);
            for (i = 0; i < Length; i++) {
                printf("\nFAIL!!! Buffer[%d] = 0x%x\n", i, Buffer[i]);
            }
        }
#endif

        ext_wifiDataFromBrickCallback(Buffer, Length);
    }

    return Length;
}

/**
 * @brief           Close the active TCP connection
 *
 * @param data      The connection data.
 * @return          OK if closing succeeded, otherwise FAIL.
 */
static RESULT cWiFiTcpClose()
{
    RESULT Result = OK;

    ext_closeTcpFromBrickCallback();

    return Result;
}

/**
 * @brief           Reset the TCP connection state.
 *
 * @param data      The connection data.
 * @return          OK on success, otherwise FAIL.
 */
static RESULT cWiFiResetTcp(void)
{
    RESULT Result;

    pr_dbg("\nRESET - client disconnected!\n");

    TcpReadState = TCP_IDLE;
    Result = cWiFiTcpClose();
    ext_startTcpFromBrickCallback();

    return Result;
}

/**
 * @brief           Reads data from the active TCP connection.
 *
 * This is a non-blocking operation, so it will return 0 if there is no data
 * to read. It also returns 0 if there is not an active connection.
 *
 * @param Buffer    A pre-allocated buffer to store the read data.
 * @param Length    The length of the buffer (number of bytes to read).
 * @return          The number of bytes actually read.
 */
UWORD cWiFiReadTcp(UBYTE* Buffer, UWORD Length)
{
    size_t DataRead = 0;

    if (1) {
        size_t read_length;

        // setup for read

        switch (TcpReadState) {
        case TCP_IDLE:
            // Do Nothing
            return 0;
        case TCP_WAIT_ON_START:
            TcpReadBufPointer = 0;
            read_length = 100; // Fixed TEXT
            break;
        case TCP_WAIT_ON_LENGTH:
            // We can should read the length of the message
            // The packets can be split from the client
            // I.e. Length bytes (2) can be send as a subset
            // the Sequence can also arrive as a single pair of bytes
            // and the finally the payload will be received

            // Begin on new buffer :-)
            TcpReadBufPointer = 0;
            read_length = 2;
            break;
        case TCP_WAIT_ON_ONLY_CHUNK:
            read_length = TcpRestLen;
            break;
        case TCP_WAIT_ON_FIRST_CHUNK:
            read_length = Length - 2;
            break;
        case TCP_WAIT_COLLECT_BYTES:
            TcpReadBufPointer = 0;
            if (TcpRestLen < Length) {
                read_length = TcpRestLen;
            } else {
                read_length = Length;
            }
            break;
        default:
            // Should never go here...
            TcpReadState = TCP_IDLE;
            return 0;
        }

        // do the actual read
        DataRead = ext_wifiDataToBrickCallback(Buffer + TcpReadBufPointer, read_length);

        if (DataRead == -1) {
            DataRead = 0;
        } else {
            // handle the read data

            switch (TcpReadState) {
            case TCP_IDLE:
                break;
            case TCP_WAIT_ON_START:
                pr_dbg("TCP_WAIT_ON_START:\n");
#ifdef DEBUG_WIFI
                printf("\nDataRead = %d, Buffer = \n", DataRead);
                if (DataRead > 0) {
                    int ii;

                    for (ii = 0; ii < DataRead; ii++) {
                        printf("0x%x, ", Buffer[ii]);
                    }
                } else {
                    printf("DataRead shows FAIL: %d", DataRead);
                }
                printf("\n");
#endif

                if (DataRead == 0) {
                    // We've a disconnect
                    cWiFiResetTcp();
                    break;
                }

                if (strstr((char*)Buffer, "ET /target?sn=") > 0) {
                    pr_dbg("\nTCP_WAIT_ON_START and  ET /target?sn= found :-)"
                           " DataRead = %d, Length = %d, Buffer = %s\n",
                           DataRead, Length, Buffer);

                    // A match found => UNLOCK
                    // Say OK back
                    cWiFiWriteTcp((UBYTE*)"Accept:EV340\r\n\r\n", 16);
                    TcpReadState = TCP_WAIT_ON_LENGTH;
                }

                DataRead = 0; // No COM-module activity yet
                break;

            case TCP_WAIT_ON_LENGTH:
                pr_dbg("TCP_WAIT_ON_LENGTH:\n");
                if (DataRead == 0) {
                    // We've a disconnect
                    cWiFiResetTcp();
                    break;
                }

                TcpRestLen = (UWORD)(Buffer[0] + Buffer[1] * 256);
                TcpTotalLength = (UWORD)(TcpRestLen + 2);
                if (TcpTotalLength > Length) {
                    TcpReadState = TCP_WAIT_ON_FIRST_CHUNK;
                } else {
                    TcpReadState = TCP_WAIT_ON_ONLY_CHUNK;
                }

                TcpReadBufPointer += DataRead;	// Position in ReadBuffer adjust
                DataRead = 0;                   // Signal NO data yet

                pr_dbg("\n*************** NEW TX *************\n");
                pr_dbg("TCP_WAIT_ON_LENGTH TcpRestLen = %d, Length = %d\n",
                       TcpRestLen, Length);

                break;

            case TCP_WAIT_ON_ONLY_CHUNK:
                pr_dbg("TCP_WAIT_ON_ONLY_CHUNK: BufferStart = %d\n",
                       TcpReadBufPointer);
                pr_dbg("DataRead = %d\n",DataRead);
                pr_dbg("BufferPointer = %p\n", &(Buffer[TcpReadBufPointer]));

                if(DataRead == 0) {
                // We've a disconnect
                    cWiFiResetTcp();
                    break;
                }

                TcpReadBufPointer += DataRead;

                if(TcpRestLen == DataRead) {
                    DataRead = TcpTotalLength; // Total count read
                    TcpReadState = TCP_WAIT_ON_LENGTH;
                } else {
                    TcpRestLen -= DataRead; // Still some bytes in this only chunk
                    DataRead = 0;           // No COMM job yet
                }
#if 0 // this makes lots of noise
                int i;

                for (i = 0; i < TcpTotalLength; i++) {
                    printf("ReadBuffer[%d] = 0x%x\n", i, Buffer[i]);
                }
#endif
                pr_dbg("TcpRestLen = %d, DataRead incl. 2 = %d, Length = %d\n",
                       TcpRestLen, DataRead, Length);

                break;

            case TCP_WAIT_ON_FIRST_CHUNK:
                pr_dbg("TCP_WAIT_ON_FIRST_CHUNK:\n");

                if(DataRead == 0) {
                    // We've a disconnect
                    cWiFiResetTcp();
                    break;
                }
                pr_dbg("DataRead = %d\n", DataRead);

                TcpRestLen -= DataRead;
                TcpReadState = TCP_WAIT_COLLECT_BYTES;
                DataRead += 2;
                pr_dbg("\nTCP_WAIT_ON_FIRST_CHUNK TcpRestLen = %d, DataRead incl."
                       " 2 = %d, Length = %d\n", TcpRestLen, DataRead, Length);

                break;

            case TCP_WAIT_COLLECT_BYTES:
                pr_dbg("TCP_WAIT_COLLECT_BYTES:\n");
                pr_dbg("DataRead = %d\n", DataRead);

                if(DataRead == 0) {
                    // We've a disconnect
                    cWiFiResetTcp();
                    break;
                }

                TcpRestLen -= DataRead;
                if(TcpRestLen == 0) {
                    TcpReadState = TCP_WAIT_ON_LENGTH;
                }
                pr_dbg("\nTCP_WAIT_COLLECT_BYTES TcpRestLen = %d, DataRead incl."
                       " 2 = %d, Length = %d\n", TcpRestLen, DataRead, Length);

                break;
            }
        }
    }

    return DataRead;
}

/**
 * @brief Gets WiFi power status
 *
 * @return OK if WiFi is present and powered on, otherwise FAIL
 */
RESULT cWiFiGetOnStatus(void)
{
    RESULT Result = FAIL;

    Result = OK;

    return Result;
}

/**
 * @brief               Read the serial number from file
 */
static void cWiFiSetBtSerialNo(void)
{
    FILE* File;

    // TODO probably
    // Get the file-based BT SerialNo
    File = fopen("./settings/BTser", "r");
    if (File) {
        fgets(BtSerialNo, BLUETOOTH_SER_LENGTH, File);
        fclose(File);
    }
}

/**
 * @brief               Read the brick name from file
 */
static void cWiFiSetBrickName(void)
{
    FILE* File;

    // TODO probably
    // Get the file-based BrickName
    File = fopen("./settings/BrickName", "r");
    if (File) {
        fgets(BrickName, BRICK_HOSTNAME_LENGTH, File);
        fclose(File);
    }
}

/**
 * @brief           Turn off WiFi
 *
 * @return          OK on success or FAIL if wifi is not present or wifi is
 *                  already turned on or turning on failed
 */
RESULT cWiFiTurnOn(void)
{
    pr_dbg("cWiFiTurnOn\n");
    // TODO probably callback hooks to c#
    return OK;
}

/**
 * @brief           Turn off WiFi
 *
 * @return          OK on success or FAIL if wifi is not present or wifi is
 *                  already turned off or turning off failed
 */
RESULT cWiFiTurnOff(void)
{
    pr_dbg("cWiFiTurnOff\n");
    // TODO probably callback hooks to c#
    return OK;
}

RESULT cWiFiExit(void)
{
    RESULT Result;

    // TODO: Do we want to always turn off WiFi on exit? This is what LEGO does.
    Result = OK; //cWiFiTurnOff();

    return Result;
}

RESULT cWiFiInit(void)
{
    RESULT Result = OK;

    pr_dbg("\ncWiFiInit START %d\n", WiFiStatus);

    // We're sleeping until user select ON
    TcpReadState = TCP_IDLE;
    cWiFiSetBtSerialNo();
    cWiFiSetBrickName();

    pr_dbg("\nWiFiStatus = %d\n", WiFiStatus);

    return Result;
}
