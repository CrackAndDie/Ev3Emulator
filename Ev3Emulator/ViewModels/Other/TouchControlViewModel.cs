using Ev3Emulator.Entities;
using Ev3Emulator.Interfaces;
using Ev3LowLevelLib;
using Hypocrite.Core.Container;
using Hypocrite.Core.Mvvm.Attributes;
using Hypocrite.Mvvm;
using Prism.Commands;
using Prism.Regions;
using System.Windows.Input;

namespace Ev3Emulator.ViewModels.Other
{
	public class TouchControlViewModel : ViewModelBase
	{
		public TouchControlViewModel()
		{
		}

		public override void OnViewReady()
		{
			base.OnViewReady();

			_navigationParameters = GetNavigationParameters<SensorViewNavigationParameters>();

			var view = GetView<ITouchControlView>();
			view.TouchPressed += OnTouchPressed;
			view.TouchReleased += OnTouchReleased;
		}

		private void OnTouchPressed()
		{
			Ev3Entity.SetTouchSensor(_navigationParameters.Port, true);
		}

		private void OnTouchReleased()
		{
			Ev3Entity.SetTouchSensor(_navigationParameters.Port, false);
		}

		[Injection]
		public Ev3Entity Ev3Entity { get; set; }

		private SensorViewNavigationParameters _navigationParameters;
	}
}
