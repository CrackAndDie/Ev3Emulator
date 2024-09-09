using Avalonia.Controls;
using Ev3Emulator.Entities;
using Ev3Emulator.Extensions;
using Ev3Emulator.Interfaces;
using Ev3LowLevelLib;
using Hypocrite.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Ev3Emulator.ViewModels
{
	public class RightMainViewModel : ViewModelBase
	{
		public RightMainViewModel()
		{
		}

		public override void OnViewReady()
		{
			base.OnViewReady();

			if (Design.IsDesignMode)
				return;


		}

		public List<SensorEntity> OutPorts { get; set; } = new List<SensorEntity>(4) 
		{
			new SensorEntity(true),
			new SensorEntity(true),
			new SensorEntity(true),
			new SensorEntity(true),
		};

		public List<SensorEntity> InPorts { get; set; } = new List<SensorEntity>(4)
		{
			new SensorEntity(false),
			new SensorEntity(false),
			new SensorEntity(false),
			new SensorEntity(false),
		};
	}
}
