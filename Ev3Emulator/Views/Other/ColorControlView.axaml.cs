using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading;
using System;
using Ev3Emulator.Interfaces;
using Avalonia.Threading;
using System.Threading.Tasks;

namespace Ev3Emulator.Views.Other;

public partial class ColorControlView : UserControl, IColorControlView
{
	private CancellationTokenSource _updateTaskCts = null;
	public event Action<byte, byte> UpdateSensor;

	public ColorControlView()
    {
        InitializeComponent();
		ambientSlider.AddHandler(Button.PointerPressedEvent, Slider_PointerPressed, handledEventsToo: true);
		ambientSlider.AddHandler(Button.PointerReleasedEvent, Slider_PointerReleased, handledEventsToo: true);
		reflectSlider.AddHandler(Button.PointerPressedEvent, Slider_PointerPressed, handledEventsToo: true);
		reflectSlider.AddHandler(Button.PointerReleasedEvent, Slider_PointerReleased, handledEventsToo: true);
	}

	private void Slider_PointerPressed(object sender, Avalonia.Input.PointerPressedEventArgs e)
	{
		_updateTaskCts = new CancellationTokenSource();
		Task.Run(async () =>
		{
			while (!_updateTaskCts.IsCancellationRequested)
			{
				Dispatcher.UIThread.Invoke(() =>
				{
					UpdateSensor?.Invoke((byte)reflectSlider.Value, (byte)ambientSlider.Value);
				});
				await Task.Delay(60);
			}
		}, _updateTaskCts.Token);
	}

	private void Slider_PointerReleased(object sender, Avalonia.Input.PointerReleasedEventArgs e)
	{
		_updateTaskCts.Cancel();
	}
}