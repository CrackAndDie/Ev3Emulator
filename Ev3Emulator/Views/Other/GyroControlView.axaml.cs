using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Ev3Emulator.Interfaces;
using System.Threading;
using System;
using Avalonia.Threading;
using System.Threading.Tasks;

namespace Ev3Emulator.Views.Other;

public partial class GyroControlView : UserControl, IGyroControlView
{
	private CancellationTokenSource _updateAngleTaskCts = null;
	public event Action<float> UpdateAngle;

	public GyroControlView()
    {
        InitializeComponent();

		gyroSlider.AddHandler(Button.PointerPressedEvent, Slider_PointerPressed, handledEventsToo: true);
		gyroSlider.AddHandler(Button.PointerReleasedEvent, Slider_PointerReleased, handledEventsToo: true);
	}

	private void Slider_PointerPressed(object sender, Avalonia.Input.PointerPressedEventArgs e)
	{
		_updateAngleTaskCts = new CancellationTokenSource();
		Task.Run(async () =>
		{
			while (!_updateAngleTaskCts.IsCancellationRequested)
			{
				Dispatcher.UIThread.Invoke(() =>
				{
					UpdateAngle?.Invoke((float)gyroSlider.Value);
				});
				await Task.Delay(60);
			}
		}, _updateAngleTaskCts.Token);
	}

	private void Slider_PointerReleased(object sender, Avalonia.Input.PointerReleasedEventArgs e)
	{
		_updateAngleTaskCts.Cancel();
		// UpdateDistance?.Invoke((float)10);
		// distSlider.Value = 10;
	}
}