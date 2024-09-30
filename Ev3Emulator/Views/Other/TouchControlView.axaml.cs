using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Hypocrite.Core.Interfaces.Presentation;
using System.Threading.Tasks;
using System.Threading;
using Ev3Emulator.Interfaces;
using System;

namespace Ev3Emulator.Views.Other;

public partial class TouchControlView : UserControl, ITouchControlView
{
    public TouchControlView()
    {
        InitializeComponent();

		touchButton.AddHandler(Button.PointerPressedEvent, Touch_PointerPressed, handledEventsToo: true);
		touchButton.AddHandler(Button.PointerReleasedEvent, Touch_PointerReleased, handledEventsToo: true);
	}

	public event Action TouchPressed;
	public event Action TouchReleased;

	private void Touch_PointerPressed(object sender, Avalonia.Input.PointerPressedEventArgs e)
	{
		TouchPressed?.Invoke();
	}

	private void Touch_PointerReleased(object sender, Avalonia.Input.PointerReleasedEventArgs e)
	{
		TouchReleased?.Invoke();
	}
}