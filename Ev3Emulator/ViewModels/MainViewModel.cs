using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Ev3Emulator.Interfaces;
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
		if (Design.IsDesignMode)
			return;

		base.OnViewReady();

		// inits
		FilesystemWrapper.Init();
		TimeWrapper.Init();
		InputWrapper.Init(); // TODO:
		MotorsWrapper.Init(); // TODO:
		SoundWrapper.Init(); // TODO:
		LcdWrapper.Init(UpdateLcd, UpdateLed);
		ButtonsWrapper.Init(UpdateButtons);
	}

	private void UpdateLcd(byte[] bmpData)
	{
		Dispatcher.UIThread.Invoke(() =>
		{
			var bmp = new WriteableBitmap(new PixelSize(LcdWrapper.vmLCD_WIDTH, LcdWrapper.vmLCD_HEIGHT), new Vector(96, 96), Avalonia.Platform.PixelFormat.Rgba8888);
			using (var frameBuffer = bmp.Lock())
			{
				// * 4 because orig data is grayscale
				Marshal.Copy(bmpData, 0, frameBuffer.Address, bmpData.Length);
			}
			LcdBitmap = bmp;
		});
	}

	private void UpdateLed(int state)
	{
		// TODO: 
	}

	private byte[] UpdateButtons()
	{
		try
		{
			// TODO: think about it. could we get not prev values but wait or smth
			Dispatcher.UIThread.Invoke(() =>
			{
				lock (_buttonsLock)
					_lastButtons = GetView<IMainView>()?.GetButtons();
			});

			lock (_buttonsLock)
				return _lastButtons ?? _defaultButtons;
		}
		catch (Exception ex)
		{
			return _defaultButtons;
		}
	}

	private void OnStartCommand()
	{
		if (Design.IsDesignMode)
			return;

		_ev3Thread = new Thread(SystemWrapper.MainLms);
		_ev3Thread.Start();
	}

	[Notify]
	public ICommand StartCommand { get; set; }

	[Notify]
    public Bitmap LcdBitmap { get; set; } 

    private Thread _ev3Thread;

	private object _buttonsLock = new object();
	private byte[] _lastButtons = _defaultButtons;
	private static readonly byte[] _defaultButtons = { 0, 0, 0, 0, 0, 0 };
}
