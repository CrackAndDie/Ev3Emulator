using EV3DecompilerLib.Decompile;
using EV3DecompilerLib.Recognize;
using EV3ModelLib;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Ev3ConsoleTest
{
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
   //         void* tst = (void*)entryPtr;
   //         float* tstF = (float*)tst;
   //         tstF[0] = 1.23f;

   //         int a = 0;
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
