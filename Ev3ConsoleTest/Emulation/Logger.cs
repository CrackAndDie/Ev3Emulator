using Ev3CoreUnsafe.Interfaces;

namespace Ev3ConsoleTest.Emulation
{
	internal class Logger : ILogger
	{
		public void Log(string message)
		{
			Log(message, null);
		}

		public void Log(string message, Exception exception)
		{
			//var prevColor = Console.ForegroundColor;
			//Console.ForegroundColor = ConsoleColor.Gray;
			//Console.WriteLine(message);
			//Console.WriteLine(exception);
			//Console.ForegroundColor = prevColor;
		}

		public void LogError(string message)
		{
			LogError(message, null);
		}

		public void LogError(string message, Exception exception)
		{
			var prevColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			Console.WriteLine(exception);
			Console.ForegroundColor = prevColor;
		}

		public void LogInfo(string message)
		{
			LogInfo(message, null);
		}

		public void LogInfo(string message, Exception exception)
		{
			//var prevColor = Console.ForegroundColor;
			//Console.ForegroundColor = ConsoleColor.Green;
			//Console.WriteLine(message);
			//Console.WriteLine(exception);
			//Console.ForegroundColor = prevColor;
		}

		public void LogWarning(string message)
		{
			LogWarning(message, null);
		}

		public void LogWarning(string message, Exception exception)
		{
			//var prevColor = Console.ForegroundColor;
			//Console.ForegroundColor = ConsoleColor.Yellow;
			//Console.WriteLine(message);
			//Console.WriteLine(exception);
			//Console.ForegroundColor = prevColor;
		}
	}
}
