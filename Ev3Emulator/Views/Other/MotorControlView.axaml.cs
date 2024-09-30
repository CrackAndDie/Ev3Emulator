using Avalonia.Controls;
using Avalonia.Threading;
using Ev3Emulator.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ev3Emulator.Views.Other;

public partial class MotorControlView : UserControl, IMotorControlView
{
	private CancellationTokenSource _updateSpeedTaskCts = null;
	public event Action<int> UpdateSpeed;

    public MotorControlView()
    {
        InitializeComponent();

		tachoSlider.AddHandler(Button.PointerPressedEvent, Slider_PointerPressed, handledEventsToo: true);
		tachoSlider.AddHandler(Button.PointerReleasedEvent, Slider_PointerReleased, handledEventsToo: true);
	}

	private void Slider_PointerPressed(object sender, Avalonia.Input.PointerPressedEventArgs e)
	{
		_updateSpeedTaskCts = new CancellationTokenSource();
		Task.Run(async () =>
		{
			while (!_updateSpeedTaskCts.IsCancellationRequested)
			{
				Dispatcher.UIThread.Invoke(() =>
				{
					UpdateSpeed?.Invoke((int)tachoSlider.Value);
				});
				await Task.Delay(300);
			}
		}, _updateSpeedTaskCts.Token);
	}

	private void Slider_PointerReleased(object sender, Avalonia.Input.PointerReleasedEventArgs e)
	{
		_updateSpeedTaskCts.Cancel();
		UpdateSpeed?.Invoke((int)0);
		tachoSlider.Value = 0;
	}
}