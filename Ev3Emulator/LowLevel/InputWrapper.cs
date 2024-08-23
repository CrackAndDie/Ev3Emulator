using System.Runtime.InteropServices;
using System;

namespace Ev3Emulator.LowLevel
{
	public unsafe struct IICDAT
	{
		public int Result;
		public sbyte Port;
		public sbyte Repeat;
		public short Time;
		public sbyte WrLng;
		public fixed sbyte WrData[32];
		public sbyte RdLng;
		public fixed sbyte RdData[32];
	}

	public unsafe struct IICSTR
	{
		public sbyte Port;
		public short Time;
		public sbyte Type;
		public sbyte Mode;
		public fixed sbyte Manufacturer[8 + 1];
		public fixed sbyte SensorType[8 + 1];
		public sbyte SetupLng;
		public uint SetupString;
		public sbyte PollLng;
		public uint PollString;
		public sbyte ReadLng;
	}

	public unsafe struct DEVCON
	{
		public fixed sbyte Connection[4];
		public fixed sbyte Type[4];
		public fixed sbyte Mode[4];
	}

	public unsafe struct UARTCTL
	{
		public TYPES TypeData;
		public sbyte Port;
		public sbyte Mode;
	}

	public unsafe struct TYPES // if data type changes - remember to change "cInputTypeDataInit" !
	{
		public fixed sbyte Name[11 + 1]; //!< Device name
		public sbyte Type;                       //!< Device type
		public sbyte Connection;
		public sbyte Mode;                       //!< Device mode
		public sbyte DataSets;
		public sbyte Format;
		public sbyte Figures;
		public sbyte Decimals;
		public sbyte Views;
		public float RawMin;                     //!< Raw minimum value      (e.c. 0.0)
		public float RawMax;                     //!< Raw maximum value      (e.c. 1023.0)
		public float PctMin;                     //!< Percent minimum value  (e.c. -100.0)
		public float PctMax;                     //!< Percent maximum value  (e.c. 100.0)
		public float SiMin;                      //!< SI unit minimum value  (e.c. -100.0)
		public float SiMax;                      //!< SI unit maximum value  (e.c. 100.0)
		public ushort InvalidTime;                //!< mS from type change to valid data
		public ushort IdValue;                    //!< Device id value        (e.c. 0 ~ UART)
		public sbyte Pins;                       //!< Device pin setup
		public fixed sbyte Symbol[4 + 1];  //!< SI unit symbol
		public ushort Align;
	}

	public static class InputWrapper
	{
		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_input_ioctlIICDAT(reg_w_input_ioctlIICDATAction ioctlIICDAT);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_input_ioctlIICDATAction(int par, IntPtr data);

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_input_ioctlIICSTR(reg_w_input_ioctlIICSTRAction ioctlIICSTR);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_input_ioctlIICSTRAction(int par, IntPtr data);

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_input_ioctlIICDEVCON(reg_w_input_ioctlIICDEVCONAction ioctlIICDEVCON);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_input_ioctlIICDEVCONAction(int par, IntPtr data);

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_input_ioctlUARTCTL(reg_w_input_ioctlUARTCTLAction ioctlUARTCTL);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_input_ioctlUARTCTLAction(int par, IntPtr data);

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_input_ioctlUARTDEVCON(reg_w_input_ioctlUARTDEVCONAction ioctlUARTDEVCON);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_input_ioctlUARTDEVCONAction(int par, IntPtr data);

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_input_writeData(reg_w_input_writeDataAction writeData);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_input_writeDataAction(int par, IntPtr data, int len);

		// TODO: custom handler post processed
		public static void Init()
		{
			reg_w_input_ioctlIICDAT(IoctlIICDAT);
			reg_w_input_ioctlIICSTR(IoctlIICSTR);
			reg_w_input_ioctlIICDEVCON(IoctlIICDEVCON);
			reg_w_input_ioctlUARTCTL(IoctlUARTCTL);
			reg_w_input_ioctlUARTDEVCON(IoctlUARTDEVCON);
			reg_w_input_writeData(WriteData);
		}

		private static void IoctlIICDAT(int par, IntPtr data)
		{
			// TODO: ...
		}

		private static void IoctlIICSTR(int par, IntPtr data)
		{
			// TODO: ...
		}

		private static void IoctlIICDEVCON(int par, IntPtr data)
		{
			// TODO: ...
		}

		private static void IoctlUARTCTL(int par, IntPtr data)
		{
			// TODO: ...
		}

		private static void IoctlUARTDEVCON(int par, IntPtr data)
		{
			// TODO: ...
		}

		private static void WriteData(int par, IntPtr data, int len)
		{
			// TODO: ...
		}
	}
}
