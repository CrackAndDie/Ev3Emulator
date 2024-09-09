using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Ev3Emulator.Extensions;
using Ev3Emulator.Interfaces;
using Ev3Emulator.LowLevel;
using Ev3LowLevelLib;
using Hypocrite.Core.Container;
using Hypocrite.Core.Mvvm.Attributes;
using Hypocrite.Mvvm;
using Prism.Commands;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Ev3Emulator.ViewModels;

public class MainViewModel : ViewModelBase
{
	public MainViewModel()
	{
	}

	public override void OnViewReady()
	{
		base.OnViewReady();

		if (Design.IsDesignMode)
			return;

		if (RegionManager.Regions["RightSideRegion"].Views.Count() == 0)
			RegionManager.ReqNav(typeof(IRightMainView), "RightSideRegion");

		// inits
		Ev3Entity.Init();
		Ev3Entity.InitLcd(UpdateLcd, UpdateLed);
        Ev3Entity.InitButtons(UpdateButtons);
        Ev3Entity.LmsExited += OnLmsVmExited;

		GetView<IMainView>().CenterButtonPressed += OnCenterButtonPressed;
		GetView<IMainView>().CenterButtonReleased += OnCenterButtonReleased;

		// resets
		OnLmsVmExited();
    }

	private void UpdateLcd(byte[] bmpData)
	{
		try
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
		catch (TaskCanceledException)
		{
			// ui is dead
		}
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
		catch (Exception)
		{
			return _defaultButtons;
		}
	}

	private void OnCenterButtonPressed()
	{
        if (Design.IsDesignMode)
            return;

        Ev3Entity.OnCenterButtonPressed();
    }

	private void OnCenterButtonReleased()
	{
        if (Design.IsDesignMode)
            return;

        Ev3Entity.OnCenterButtonReleased();
    }

	private void OnLmsVmExited()
	{
		UpdateLcd(LcdWrapper.ConvertToRgba8888(new byte[LcdWrapper.vmLCD_WIDTH * LcdWrapper.vmLCD_HEIGHT], true));
	}

	[Injection]
	public Ev3Entity Ev3Entity { get; set; }

	[Notify]
    public Bitmap LcdBitmap { get; set; } 
    
	private object _buttonsLock = new object();
	private byte[] _lastButtons = _defaultButtons;
	private static readonly byte[] _defaultButtons = { 0, 0, 0, 0, 0, 0 };
}
