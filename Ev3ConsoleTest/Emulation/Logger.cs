using Ev3CoreUnsafe.Interfaces;

namespace Ev3ConsoleTest.Emulation
{
	internal class Logger : ILogger
	{
		internal Logger()
		{
			if (File.Exists("anime.txt"))
				File.Delete("anime.txt");
		}

		private void LogInternal(string message, Exception exception, bool onlyFile = false)
		{
			if (!onlyFile)
			{
				Console.WriteLine(message);
				Console.WriteLine(exception);
			}
			File.AppendAllText("anime.txt", message + '\n');
			if (exception != null)
				File.AppendAllText("anime.txt", exception.ToString() + '\n');
		}

		public void Log(string message)
		{
			Log(message, null);
		}

		public void Log(string message, Exception exception)
		{
			var prevColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Gray;
			LogInternal(message, exception, true);
			Console.ForegroundColor = prevColor;
		}

		public void LogError(string message)
		{
			LogError(message, null);
		}

		public void LogError(string message, Exception exception)
		{
			var prevColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			LogInternal(message, exception);
			Console.ForegroundColor = prevColor;
		}

		public void LogInfo(string message)
		{
			LogInfo(message, null);
		}

		public void LogInfo(string message, Exception exception)
		{
			var prevColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Green;
			LogInternal(message, exception, true);
			Console.ForegroundColor = prevColor;
		}

		public void LogWarning(string message)
		{
			LogWarning(message, null);
		}

		public void LogWarning(string message, Exception exception)
		{
			var prevColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Yellow;
			LogInternal(message, exception, true);
			Console.ForegroundColor = prevColor;
		}
	}
}
