using EV3DecompilerLib.Decompile;
using EV3DecompilerLib.Recognize;
using EV3ModelLib;

namespace Ev3ConsoleTest
{
    internal class Program
    {
        static void Main(string[] args)
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
    }
}
