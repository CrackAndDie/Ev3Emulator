using Avalonia.Media;
using Avalonia.Media.Imaging;
using Ev3CoreUnsafe.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;

namespace Ev3Emulator.CoreImpl
{
	public class LogData
	{
		public string Text { get; set; }
		public IBrush Color { get; set; }
	}

	internal class ViewLogger : ILogger
	{
		public Action<LogData> LogAction { get; set; }

		private void LogInternal(LogData logData)
		{
			LogAction?.Invoke(logData);
			Thread.Sleep(40);
		}

		public void Log(string message)
		{
			Log(message, null);
		}

		public void Log(string message, Exception exception)
		{
			var data = new LogData();
			data.Color = Brushes.Gray;
			StringBuilder sb = new StringBuilder();
			sb.Append(message);
			if (exception != null)
				sb.Append(exception.ToString());
			data.Text = sb.ToString();
			LogInternal(data);
		}

		public void LogError(string message)
		{
			LogError(message, null);
		}

		public void LogError(string message, Exception exception)
		{
			var data = new LogData();
			data.Color = Brushes.Red;
			StringBuilder sb = new StringBuilder();
			sb.Append(message);
			if (exception != null)
				sb.Append(exception.ToString());
			data.Text = sb.ToString();
			LogInternal(data);
		}

		public void LogInfo(string message)
		{
			LogInfo(message, null);
		}

		public void LogInfo(string message, Exception exception)
		{
			var data = new LogData();
			data.Color = Brushes.Green;
			StringBuilder sb = new StringBuilder();
			sb.Append(message);
			if (exception != null)
				sb.Append(exception.ToString());
			data.Text = sb.ToString();
			LogInternal(data);
		}

		public void LogWarning(string message)
		{
			LogWarning(message, null);
		}

		public void LogWarning(string message, Exception exception)
		{
			var data = new LogData();
			data.Color = Brushes.Yellow;
			StringBuilder sb = new StringBuilder();
			sb.Append(message);
			if (exception != null)
				sb.Append(exception.ToString());
			data.Text = sb.ToString();
			LogInternal(data);
		}
	}
}
