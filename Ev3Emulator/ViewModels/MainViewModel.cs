using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Ev3Emulator.LowLevel;
using Hypocrite.Core.Mvvm.Attributes;
using Hypocrite.Mvvm;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Input;

namespace Ev3Emulator.ViewModels;

public class MainViewModel : ViewModelBase
{
	public MainViewModel()
	{
		StartCommand = new DelegateCommand(OnStartCommand);
	}

	public override void OnViewReady()
	{
		base.OnViewReady();

		if (Design.IsDesignMode)
			return;

		(GH.Ev3System.LcdHandler as LcdHandler).BitmapAction = UpdateLcd;
	}

	private void UpdateLcd(Bitmap bmp)
	{
        Dispatcher.UIThread.Invoke(() =>
        {
            LcdBitmap = bmp;
        });
    }

	private void OnStartCommand()
	{
		_ev3Thread = new Thread(SystemWrapper.Main);
		_ev3Thread.Start();
	}

	[Notify]
	public ICommand StartCommand { get; set; }

	[Notify]
    public Bitmap LcdBitmap { get; set; }

	private Thread _ev3Thread;
}
