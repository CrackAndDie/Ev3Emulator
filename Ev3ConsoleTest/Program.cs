using Ev3ConsoleTest;
using EV3DecompilerLib.Decompile;
using EV3DecompilerLib.Recognize;
using EV3ModelLib;
using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ev3ConsoleTest
{
	public unsafe struct TYPES // if data type changes - remember to change "cInputTypeDataInit" !
	{
		public fixed byte Name[11 + 1]; //!< Device name
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

	internal class Program
    {
        static unsafe void Main(string[] args)
        {
			//string path = @"./TestPrograms/Program.rbf";
			//var data = File.ReadAllBytes(path);
			//string text = GetText(data, path);
			//Console.WriteLine(text);
			//Console.ReadKey();

			//var entry = GC.AllocateArray<byte>(10, pinned: true);
			//var entryPtr = (byte*)Unsafe.AsPointer(ref entry[0]);
			//void* tst = (void*)entryPtr;
			//float* tstF = (float*)tst;
			//tstF[0] = 1.23f;
			//int a = 0;

			//         byte mmm = 2;
			//var entry = GC.AllocateArray<byte>(16, pinned: true);
			//var entryPtr = (byte*)Unsafe.AsPointer(ref entry[0]);
			//void* tst = (void*)entryPtr;
			//B* tstF = (B*)tst;
			//         // tstF->x = &mmm;
			//         tstF->y = 3;
			//         tstF->z = 4;
			//         int sma = Unsafe.SizeOf<A>();
			//         int sm = Unsafe.SizeOf<B>();
			//int a = 0;

			//var entry = GC.AllocateArray<byte>(16, pinned: true);
			//var entryPtr = (byte*)Unsafe.AsPointer(ref entry[0]);
			//entryPtr[12] = 4;
			//entryPtr[13] = 5;
			//void* tst = (void*)entryPtr;
			//B2* tstF = (B2*)tst;
			//int sm = Unsafe.SizeOf<B2>();
			//int a = 0;

			//A* tstA = (A*)tst;

			//A* ma = (A*)Unsafe.AsPointer(ref gh.C.a);
			//B* m = (B*)Unsafe.AsPointer(ref gh.C.b);
			//ma->y = 3;
			//m->y = 3;

			var a = -1;
			ushort b = (ushort)a;

			var a3 = sizeof(TYPES);
		}

		private static string GetText(byte[] data, string path)
        {
            var bytecode = new Decompiler().BuildByteCodeForData(data, Path.GetFileName(path));
            var recognizedEV3Blocks = new PatternMatcher().RecognizeEV3Calls(bytecode, Path.GetFileName(path));
            PrintOptionsClass poc = new PrintOptionsClass()
            {
                TargetColorIndex = PrintOptionsClass.TargetColorEnum.None,
            };
            Node.PrintOptions = poc;
            var nd = NodeConversion.Convert(recognizedEV3Blocks.program);

            return PrintHelper.GetPMDListString(nd);
        }
    }
}
