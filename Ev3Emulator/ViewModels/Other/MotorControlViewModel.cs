using Ev3Emulator.Entities;
using Ev3Emulator.Interfaces;
using Ev3LowLevelLib;
using Hypocrite.Core.Container;
using Hypocrite.Core.Mvvm.Attributes;
using Hypocrite.Mvvm;
using Prism.Regions;

namespace Ev3Emulator.ViewModels.Other
{
	public class MotorControlViewModel : ViewModelBase
	{
		public override void OnViewReady()
		{
			base.OnViewReady();

			Ev3Entity.MotorSpeedChanged += OnMotorSpeedChanged;

			_navigationParameters = GetNavigationParameters<SensorViewNavigationParameters>();

			GetView<IMotorControlView>().UpdateSpeed += (v) =>
			{
				Ev3Entity.SetMotorSpeed(_navigationParameters.Port, v);
			};
		}

		public override void OnNavigatedFrom(NavigationContext navigationContext)
		{
			base.OnNavigatedFrom(navigationContext);

			Ev3Entity.MotorSpeedChanged -= OnMotorSpeedChanged;
		}

		private void OnMotorSpeedChanged(int port, int speed)
		{
			if (_navigationParameters.Port != port)
				return;

			CurrentMotorSpeed = speed;
		}

		[Injection]
		public Ev3Entity Ev3Entity { get; set; }

		[Notify]
		public int CurrentMotorSpeed { get; set; }

		private SensorViewNavigationParameters _navigationParameters;
	}
}
