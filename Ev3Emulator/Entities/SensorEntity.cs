using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Hypocrite.Core.Mvvm;
using Hypocrite.Core.Mvvm.Attributes;
using System.Collections.Generic;

namespace Ev3Emulator.Entities
{
	public class SensorEntity : BindableObject
	{
		public SensorEntity(bool isOut)
		{
			_isOut = isOut;	
		}

		private bool _isOut = false;

		private const string _pathToIcons = "avares://Ev3Emulator/Resources/SensorIcons";
		public static Bitmap[] AllAvailableSensors =
		{
			new Bitmap(AssetLoader.Open(new System.Uri($"{_pathToIcons}/other.bmp"))),
			new Bitmap(AssetLoader.Open(new System.Uri($"{_pathToIcons}/touch.bmp"))),
			new Bitmap(AssetLoader.Open(new System.Uri($"{_pathToIcons}/us_sens.bmp"))),
			new Bitmap(AssetLoader.Open(new System.Uri($"{_pathToIcons}/ir_sens.bmp"))),
			new Bitmap(AssetLoader.Open(new System.Uri($"{_pathToIcons}/gyro_sens.bmp"))),
			new Bitmap(AssetLoader.Open(new System.Uri($"{_pathToIcons}/color_sens.bmp"))),
		};

		public static Bitmap[] AllAvailableMotors =
		{
			new Bitmap(AssetLoader.Open(new System.Uri($"{_pathToIcons}/other.bmp"))),
			new Bitmap(AssetLoader.Open(new System.Uri($"{_pathToIcons}/lrg_motor.bmp"))),
			new Bitmap(AssetLoader.Open(new System.Uri($"{_pathToIcons}/med_motor.bmp"))),
		};

		public Bitmap[] AllSensors => _isOut ? AllAvailableMotors : AllAvailableSensors;

		[Notify]
		public int SelectedSensor { get; set; }
	}
}
