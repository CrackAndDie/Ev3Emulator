using Ev3ConsoleTest;
using Ev3ConsoleTest.Emulation;
using Ev3CoreUnsafe;
using Ev3CoreUnsafe.Ccom.Interfaces;
using Ev3CoreUnsafe.Helpers;
using Ev3CoreUnsafe.Lms2012.Interfaces;
using EV3DecompilerLib.Decompile;
using EV3DecompilerLib.Recognize;
using EV3ModelLib;
using System.Runtime.CompilerServices;

namespace Ev3ConsoleTest
{
	internal unsafe class Program
    {
        static unsafe void Main(string[] args)
        {
            // EnumlatorTest();
            CommonTest();

            Console.ReadKey();
		}

		#region common test
        private unsafe static void CommonTest()
        {
			byte* notAllocated = CommonHelper.AllocateByteArray(3);

			// notAllocated[4] = 3;

			CommonHelper.DeleteByteArray(&notAllocated[3]);

            System.Console.WriteLine("asdasd");
            System.Console.WriteLine("asdasd");
            System.Console.WriteLine("asdasd");
            System.Console.WriteLine("asdasd");
            System.Console.WriteLine("asdasd");
		}
		#endregion

		#region emulator test
		private static void EnumlatorTest()
        {
			Ev3System syst = new Ev3System();
            GH.Ev3System = syst;
			GH.Main();
        }
		#endregion

		#region decompile test
		private static void DecompileTest()
        {
            string path = @"./TestPrograms/Program.rbf";
            var data = File.ReadAllBytes(path);
            string text = GetText(data, path);
            Console.WriteLine(text);
            Console.ReadKey();
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
		#endregion
	}
}
