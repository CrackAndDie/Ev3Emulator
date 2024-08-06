namespace Ev3CoreUnsafe.Interfaces
{
	public interface ILogger
	{
		/// <summary>
		/// Debug log
		/// </summary>
		/// <param name="message"></param>
		void Log(string message);
		/// <summary>
		/// Debug log
		/// </summary>
		/// <param name="message"></param>
		/// <param name="exception"></param>
		void Log(string message, Exception exception);


		void LogInfo(string message);
		void LogInfo(string message, Exception exception);
		void LogWarning(string message);
		void LogWarning(string message, Exception exception);
		void LogError(string message);
		void LogError(string message, Exception exception);
	}
}
