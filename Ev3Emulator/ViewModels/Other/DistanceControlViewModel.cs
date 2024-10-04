using Ev3Emulator.Entities;
using Ev3Emulator.Interfaces;
using Ev3LowLevelLib;
using Hypocrite.Core.Container;
using Hypocrite.Core.Mvvm.Attributes;
using Hypocrite.Mvvm;
using Prism.Regions;

namespace Ev3Emulator.ViewModels.Other
{
	public class DistanceControlViewModel : ViewModelBase
	{
		public override void OnViewReady()
		{
			base.OnViewReady();

			_navigationParameters = GetNavigationParameters<SensorViewNavigationParameters>();

			if (_navigationParameters.IsSonic)
			{
				MaxDistance = 255;
				Ev3Entity.InitUsSensor(_navigationParameters.Port, GetUsDistance);
			}
			else
			{
				MaxDistance = 100;
				Ev3Entity.InitIrSensor(_navigationParameters.Port, GetIrDistance);
			}

			CurrentDistance = 10;
			GetView<IDistanceControlView>().UpdateDistance += (v) =>
			{
				CurrentDistance = v;
			};
		}

		public override void OnNavigatedFrom(NavigationContext navigationContext)
		{
			base.OnNavigatedFrom(navigationContext);

			if (_navigationParameters.IsSonic)
				Ev3Entity.ResetUsSensor(_navigationParameters.Port);
			else
				Ev3Entity.ResetIrSensor(_navigationParameters.Port);
		}

		private float GetUsDistance()
		{
			return CurrentDistance;
		}

		private sbyte GetIrDistance()
		{
			return (sbyte)CurrentDistance;
		}

		[Injection]
		public Ev3Entity Ev3Entity { get; set; }

		[Notify]
		public float CurrentDistance { get; set; }

		[Notify]
		public float MaxDistance { get; set; }

		private SensorViewNavigationParameters _navigationParameters;
	}
}
