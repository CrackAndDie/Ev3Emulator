using Avalonia.Controls;
using Ev3Emulator.Extensions;
using Ev3Emulator.Interfaces;
using Ev3LowLevelLib;
using Hypocrite.Mvvm;
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
	}
}
