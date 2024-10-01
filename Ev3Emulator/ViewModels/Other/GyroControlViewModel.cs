using Ev3Emulator.Entities;
using Ev3Emulator.Interfaces;
using Ev3LowLevelLib;
using Hypocrite.Core.Container;
using Hypocrite.Core.Mvvm.Attributes;
using Hypocrite.Mvvm;
using Prism.Regions;

namespace Ev3Emulator.ViewModels.Other
{
	public class GyroControlViewModel : ViewModelBase
	{
		public override void OnViewReady()
		{
			base.OnViewReady();

			_navigationParameters = GetNavigationParameters<SensorViewNavigationParameters>();

			Ev3Entity.InitGyroSensor(_navigationParameters.Port, GetGyroAngle);

			GetView<IGyroControlView>().UpdateAngle += (v) =>
			{
				CurrentRotation = v;
			};
		}

		public override void OnNavigatedFrom(NavigationContext navigationContext)
		{
			base.OnNavigatedFrom(navigationContext);

			Ev3Entity.ResetGyroSensor(_navigationParameters.Port);
		}

		private short GetGyroAngle()
		{
			return (short)CurrentRotation;
		}

		[Injection]
		public Ev3Entity Ev3Entity { get; set; }

		[Notify]
		public float CurrentRotation { get; set; }

		private SensorViewNavigationParameters _navigationParameters;
	}
}
