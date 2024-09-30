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

			Ev3Entity.InitUsSensor(_navigationParameters.Port, GetUsDistance);

			GetView<IDistanceControlView>().UpdateDistance += (v) =>
			{
				CurrentDistance = v;
			};
		}

		public override void OnNavigatedFrom(NavigationContext navigationContext)
		{
			base.OnNavigatedFrom(navigationContext);

			Ev3Entity.ResetUsSensor(_navigationParameters.Port);
		}

		private float GetUsDistance()
		{
			return CurrentDistance;
		}

		[Injection]
		public Ev3Entity Ev3Entity { get; set; }

		[Notify]
		public float CurrentDistance { get; set; }

		private SensorViewNavigationParameters _navigationParameters;
	}
}
