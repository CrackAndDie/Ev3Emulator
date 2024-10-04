using Avalonia;
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
				CurrentReflect = r;
				CurrentAmbient = a;
			};

			RaisePropertyChanged(nameof(AllColors));
		}

		public override void OnNavigatedFrom(NavigationContext navigationContext)
		{
			base.OnNavigatedFrom(navigationContext);

			Ev3Entity.ResetColorSensor(_navigationParameters.Port);
		}

		private (byte, byte, byte) GetColorData()
		{
			return ((byte)SelectedColor, CurrentReflect, CurrentAmbient);
		}

		[Injection]
		public Ev3Entity Ev3Entity { get; set; }

		[Notify]
		public int SelectedColor { get; set; } = 0;
		[Notify]
		public byte CurrentReflect { get; set; } = 0;
		[Notify]
		public byte CurrentAmbient { get; set; } = 0;

		public IBrush[] AllColors { get; set; } = new IBrush[] 
		{
			_noneColor,
			Brushes.Black, 
			Brushes.Blue, 
			Brushes.Green, 
			Brushes.Yellow, 
			Brushes.Red, 
			Brushes.White, 
			Brushes.Brown, 
		};

		private static IBrush _noneColor = new LinearGradientBrush()
		{
			StartPoint = RelativePoint.Parse("0%,0%"),
			EndPoint = RelativePoint.Parse("100%,100%"),
			GradientStops = new GradientStops()
			{
				new GradientStop(Colors.AliceBlue, 0),
				new GradientStop(Colors.AliceBlue, 0.47),
				new GradientStop(Colors.Red, 0.5),
				new GradientStop(Colors.AliceBlue, 0.53),
				new GradientStop(Colors.AliceBlue, 1),
			}
		};

		private SensorViewNavigationParameters _navigationParameters;
	}
}
