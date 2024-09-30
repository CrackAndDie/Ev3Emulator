using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Ev3Emulator.Interfaces;
using System.Threading;
using System;
using Avalonia.Threading;
using System.Threading.Tasks;

namespace Ev3Emulator.Views.Other;

public partial class DistanceControlView : UserControl, IDistanceControlView
{
	private CancellationTokenSource _updateDistanceTaskCts = null;
	public event Action<float> UpdateDistance;

	public DistanceControlView()
    {
        InitializeComponent();

		distSlider.AddHandler(Button.PointerPressedEvent, Slider_PointerPressed, handledEventsToo: true);
		distSlider.AddHandler(Button.PointerReleasedEvent, Slider_PointerReleased, handledEventsToo: true);
	}

	private void Slider_PointerPressed(object sender, Avalonia.Input.PointerPressedEventArgs e)
	{
		_updateDistanceTaskCts = new CancellationTokenSource();
		Task.Run(async () =>
		{
			while (!_updateDistanceTaskCts.IsCancellationRequested)
			{
				Dispatcher.UIThread.Invoke(() =>
				{
					UpdateDistance?.Invoke((float)distSlider.Value);
				});
				await Task.Delay(300);
			}
		}, _updateDistanceTaskCts.Token);
	}

	private void Slider_PointerReleased(object sender, Avalonia.Input.PointerReleasedEventArgs e)
	{
		_updateDistanceTaskCts.Cancel();
		UpdateDistance?.Invoke((float)10);
		distSlider.Value = 10;
	}
}