using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Ev3Emulator.Extensions;
using Ev3Emulator.Interfaces;
using Ev3LowLevelLib;
using Hypocrite.Core.Extensions;
using Hypocrite.Core.Mvvm;
using Hypocrite.Core.Mvvm.Attributes;
using Hypocrite.Mvvm;
using System.Collections.Generic;

namespace Ev3Emulator.Entities
{
	public class SensorEntity : ViewModelBase
	{
		public SensorEntity(bool isOut, int index)
		{
			_isOut = isOut;
			Index = index;

			this.WhenPropertyChanged(x => x.SelectedSensor).Subscribe(OnSelectedSensorChanged);
		}

		private void OnSelectedSensorChanged(int sens)
		{
			RaisePropertyChanged(nameof(SensorItself));

			RegionManager.Regions[RegionName].RemoveAll();
			switch (SelectedSensorType)
			{
				case SensorType.None:
					break;
				case SensorType.MediumMotor:
				case SensorType.LargeMotor:
					RegionManager.ReqNav(typeof(IMotorControlView), RegionName, new SensorViewNavigationParameters() { Port = Index });
					break;
				case SensorType.TouchSensor:
					RegionManager.ReqNav(typeof(ITouchControlView), RegionName, new SensorViewNavigationParameters() { Port = Index });
					break;
				case SensorType.UsSensor:
					RegionManager.ReqNav(typeof(IDistanceControlView), RegionName, new SensorViewNavigationParameters() { Port = Index, IsSonic = true });
					break;
				case SensorType.IrSensor:
					RegionManager.ReqNav(typeof(IDistanceControlView), RegionName, new SensorViewNavigationParameters() { Port = Index });
					break;
				case SensorType.GyroSensor:
					RegionManager.ReqNav(typeof(IGyroControlView), RegionName, new SensorViewNavigationParameters() { Port = Index });
					break;
				case SensorType.ColorSensor:
					RegionManager.ReqNav(typeof(IColorControlView), RegionName, new SensorViewNavigationParameters() { Port = Index });
					break;
			}
		}

		private bool _isOut = false;
		public int Index { get; set; }

		public string RegionName => (_isOut ? "Out" : "In") + $"{Index}Region";

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

		public SensorType SelectedSensorType 
		{
			get 
			{
				if (_isOut)
				{
					switch (SelectedSensor)
					{
						case 1:
							return SensorType.LargeMotor;
						case 2:
							return SensorType.MediumMotor;
					}
				}
				else
				{
					switch (SelectedSensor)
					{
						case 1:
							return SensorType.TouchSensor;
						case 2:
							return SensorType.UsSensor;
						case 3:
							return SensorType.IrSensor;
						case 4:
							return SensorType.GyroSensor;
						case 5:
							return SensorType.ColorSensor;
					}
				}
				return SensorType.None;
			} 
		}

		public SensorEntity SensorItself => this;
	}
}
