using Ev3Core.Enums;

namespace Ev3Core.Ccom.Interfaces
{
	public interface IDaisy
	{
		RESULT cDaisyReady();
		int cDaisyWrite();                              // Write (ASYNC) one packet DOWNSTREAM
		RESULT cDaisyGetDownstreamData(DATA8 Layer, DATA8 Sensor, DATA8 Length, VarPointer<DATA8> pType, VarPointer<DATA8> pMode, ArrayPointer<UBYTE> pData);
		void cDaisyPollFromDownstream();                 // Read (ASYNC) one packet UPSTREAM
		RESULT cDaisyDownStreamCmd(ArrayPointer<UBYTE> pData, DATA8 Length, DATA8 Layer);    // DaisyChained commands e.g. Motor cmd's																																// (up from a lower layer)
		int cDaisyWriteDone();                                                // Async check INTERNAL for Write finished
		int cDaisyGetLastWriteState();                            // Async check used for EXTERNAL check for Write finished
		int cDaisyGetLastWriteResult();                             // Get last ERROR code (or success ;-))
		RESULT cDaisyInit();                                                        // Initialize LIBUSB and open an USB DaisyChain link
		RESULT cDaisyOpen();                                                        // Open() the DaisyChain link to the slave
		RESULT cDaisyClose();                                                     // Close() the DaisyChain stuff
		RESULT cDaisyExit();                                                        // Close the DaisyChain link and deinitializes the LIBUSB
		void cDaisyControl();                                             // Get cycles for the DAISY stuff
		void cDaisySetTimeout(int TimeOut);                           // TimeOut value for read and write
		DATA8 cDaisyGetUsbUpStreamSpeed();                // The connection speed upstream (Gadget side)
		void cDaisyReadFromDownStream();                 // Read array and hand over upstream
		int cDaisyGetInterruptPacketSize();             // Max packet the Daisychained device supports
		void cDaisyCmd(RXBUF pRxBuf, TXBUF pTxBuf);        // "Internal" Daisy and daisy-chained commands
		RESULT cDaisySetDeviceInfo(DATA8 Length, ArrayPointer<UBYTE> pInfo); // Device Info pushed by INPUT module
		RESULT cDaisyGetDeviceInfo(DATA8 Length, ArrayPointer<UBYTE> pInfo); // Get Device Info for upstream TX
		RESULT cDaisySetDeviceType(DATA8 Layer, DATA8 Port, DATA8 Type, DATA8 Mode);
		UWORD cDaisyData(ArrayPointer<ArrayPointer<UBYTE>> pData);                        // Separate buffered DaisyData - NOT conflicting with normal transfer e.g. answers, errors etc.
		void cDaisyStuffTxed();                         // Tell when bits'n'bytes has left first (our) BUFFER
		uint GetDaisyPushCounter();
		void ResetDaisyPushCounter();
		RESULT cDaisyChained();
		void DecrementDaisyPushCounter();
		void cDaisyPushUpStream();                       // Flood upward
		void cDaisyPrepareNext();
		void SetUnlocked(int Status);
		int GetUnlocked();
		void SetSlaveUnlocked(int Status);
		int GetSlaveUnlocked();
		UBYTE cDaisyGetOwnLayer();
		UBYTE cDaisyCheckBusyBit(UBYTE Layer, UBYTE PortBits);  // Used by the "ONFOR" motor stuff
		RESULT cDaisyCheckBusyIndex(UBYTE Layer, UBYTE Port);
		void cDaisySetBusyFlags(UBYTE Layer, UBYTE Port, UBYTE MagicCookie);
		void cDaisySetOwnLayer(UBYTE Layer);                   // The nice to known - used for "self"-layer insertion downstream
		RESULT cDaisyMotorDownStream(ArrayPointer<UBYTE> pData, DATA8 Length, DATA8 Layer, DATA8 PortField); // Looks like the normal CMD, but
																								// port field added - i.e. NOT too much
																								// specific protocol down @ the transport layer
	}
}
