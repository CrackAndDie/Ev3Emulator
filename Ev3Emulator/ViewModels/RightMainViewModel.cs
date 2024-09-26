using Avalonia.Controls;
using Ev3Emulator.Entities;
using Ev3Emulator.Extensions;
using Ev3Emulator.Interfaces;
using Ev3LowLevelLib;
using Hypocrite.Core.Container;
using Hypocrite.Core.Extensions;
using Hypocrite.Localization;
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
			OutPorts[0].WhenPropertyChanged(x => x.SensorItself).Subscribe(OnOutPortChanged);
			OutPorts[1].WhenPropertyChanged(x => x.SensorItself).Subscribe(OnOutPortChanged);
			OutPorts[2].WhenPropertyChanged(x => x.SensorItself).Subscribe(OnOutPortChanged);
			OutPorts[3].WhenPropertyChanged(x => x.SensorItself).Subscribe(OnOutPortChanged);

			InPorts[0].WhenPropertyChanged(x => x.SensorItself).Subscribe(OnInPortChanged);
			InPorts[1].WhenPropertyChanged(x => x.SensorItself).Subscribe(OnInPortChanged);
			InPorts[2].WhenPropertyChanged(x => x.SensorItself).Subscribe(OnInPortChanged);
			InPorts[3].WhenPropertyChanged(x => x.SensorItself).Subscribe(OnInPortChanged);
		}

		public override void OnViewReady()
		{
			base.OnViewReady();

			if (Design.IsDesignMode)
				return;
		}

		private void OnOutPortChanged(SensorEntity sens)
		{
			Ev3Entity.SetOutPort(sens.Index, sens.SelectedSensorType);
		}

		private void OnInPortChanged(SensorEntity sens)
		{
			Ev3Entity.SetInPort(sens.Index, sens.SelectedSensorType);
		}

		public List<SensorEntity> OutPorts { get; set; } = new List<SensorEntity>(4) 
		{
			new SensorEntity(true, 0),
			new SensorEntity(true, 1),
			new SensorEntity(true, 2),
			new SensorEntity(true, 3),
		};

		public List<SensorEntity> InPorts { get; set; } = new List<SensorEntity>(4)
		{
			new SensorEntity(false, 0),
			new SensorEntity(false, 1),
			new SensorEntity(false, 2),
			new SensorEntity(false, 3),
		};

		[Injection]
		public Ev3Entity Ev3Entity { get; set; }
	}
}
