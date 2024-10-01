using Prism.Regions;

namespace Ev3Emulator.Entities
{
	public class SensorViewNavigationParameters
	{
		public int Port { get; set; }
		public bool IsSonic { get; set; } // used only for DistanceView
	}
}
