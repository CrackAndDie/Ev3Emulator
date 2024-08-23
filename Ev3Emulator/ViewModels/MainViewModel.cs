using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Ev3Emulator.LowLevel;
using Hypocrite.Core.Mvvm.Attributes;
using Hypocrite.Mvvm;
using Prism.Commands;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Input;
using System.Xml.Linq;

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

		// inits
		FilesystemWrapper.Init();
		TimeWrapper.Init();
		InputWrapper.Init(); // TODO:
		MotorsWrapper.Init(); // TODO:
		SoundWrapper.Init(); // TODO:
		LcdWrapper.Init(UpdateLcd, UpdateLed);
		ButtonsWrapper.Init(UpdateButtons);
	}

	private void UpdateLcd(IntPtr buf, int size)
	{
		var bmp = LcdWrapper.GetBitmap(buf, size);

		Dispatcher.UIThread.Invoke(() =>
        {
            LcdBitmap = bmp;
        });
    }

	private void UpdateLed(int state)
	{
		// TODO: 
	}

	private IntPtr UpdateButtons()
	{
		// TODO:
		var bytes = new byte[] { 0, 0, 0, 0, 0, 0 };
		IntPtr p = Marshal.AllocHGlobal(bytes.Length);
		Marshal.Copy(bytes, 0, p, bytes.Length);
		return p;
	}

	private void OnStartCommand()
	{
		_ev3Thread = new Thread(SystemWrapper.MainLms);
		_ev3Thread.Start();
	}

	[Notify]
	public ICommand StartCommand { get; set; }

	[Notify]
    public Bitmap LcdBitmap { get; set; }

	private Thread _ev3Thread;
}
