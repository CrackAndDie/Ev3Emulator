using Ev3ConsoleTest;
using Ev3ConsoleTest.Emulation;
using Ev3CoreUnsafe;
using EV3DecompilerLib.Decompile;
using EV3DecompilerLib.Recognize;
using EV3ModelLib;

namespace Ev3ConsoleTest
{
    public unsafe struct IMGHEAD
    {
        public fixed byte Sign[4];   //!< Place holder for the file type identifier                  
        public uint ImageSize;  //!< Image size                  
        public ushort VersionInfo;  //!< Version identifier                 
        public ushort NumberOfObjects;  //!< Total number of objects in image          
        public uint GlobalBytes;  //!< Number of bytes to allocate for global variables
    }

    internal class Program
    {
        static unsafe void Main(string[] args)
        {
            EnumlatorTest();
            // CommonTest();
		}

		#region common test
        private unsafe static void CommonTest()
        {
            var sz = sizeof(IMGHEAD);
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
