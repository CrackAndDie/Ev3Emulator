using Avalonia.Media;
using Ev3Emulator.Entities;
using Ev3Emulator.Interfaces;
using Ev3LowLevelLib;
using Hypocrite.Core.Container;
using Hypocrite.Core.Mvvm.Attributes;
using Hypocrite.Mvvm;
using Prism.Regions;

namespace Ev3Emulator.ViewModels.Other
{
	public class ColorControlViewModel : ViewModelBase
	{
		public override void OnViewReady()
		{
			base.OnViewReady();

			_navigationParameters = GetNavigationParameters<SensorViewNavigationParameters>();

			Ev3Entity.InitColorSensor(_navigationParameters.Port, GetColorData);

			GetView<IColorControlView>().UpdateSensor += (r, a) =>
			{
				_currentReflect = r;
				_currentAmbient = a;
			};
		}

		public override void OnNavigatedFrom(NavigationContext navigationContext)
		{
			base.OnNavigatedFrom(navigationContext);

			Ev3Entity.ResetColorSensor(_navigationParameters.Port);
		}

		private (byte, byte, byte) GetColorData()
		{
			return ((byte)SelectedColor, _currentReflect, _currentAmbient);
		}

		[Injection]
		public Ev3Entity Ev3Entity { get; set; }

		[Notify]
		public int SelectedColor { get; set; } = 0;

		public IBrush[] AllColors = new IBrush[] 
		{ 
			Brushes.Firebrick, // TODO: no color
			Brushes.Black, 
			Brushes.Blue, 
			Brushes.Green, 
			Brushes.Yellow, 
			Brushes.Red, 
			Brushes.White, 
			Brushes.Brown, 
		};

		private SensorViewNavigationParameters _navigationParameters;
		private byte _currentReflect = 0;
		private byte _currentAmbient = 0;
	}
}
