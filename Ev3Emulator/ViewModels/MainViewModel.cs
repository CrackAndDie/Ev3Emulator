﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Ev3CoreUnsafe;
using Ev3CoreUnsafe.Helpers;
using Ev3Emulator.CoreImpl;
using Hypocrite.Core.Mvvm.Attributes;
using Hypocrite.Mvvm;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
		(GH.Ev3System.Logger as ViewLogger).LogAction = UpdateLog;
	}

	private void UpdateLog(LogData data)
	{
		Dispatcher.UIThread.Invoke(() =>
		{
			OutputData.Add(data);
			LastData = data;
		});
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
		_ev3Thread = new Thread(GH.Main);
		_ev3Thread.Start();
	}

	[Notify]
	public ICommand StartCommand { get; set; }

	public ObservableCollection<LogData> OutputData { get; } = new ObservableCollection<LogData>();

	[Notify]
    public Bitmap LcdBitmap { get; set; }
	[Notify]
	public LogData LastData { get; set; }

	private Thread _ev3Thread;
}
