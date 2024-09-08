using Avalonia.Controls;
using Ev3Emulator.Interfaces;
using System;

namespace Ev3Emulator.Views;

public partial class MainView : UserControl, IMainView
{
    public event Action CenterButtonPressed;
    public event Action CenterButtonReleased;

    public MainView()
    {
        InitializeComponent();

        buttonCenter.AddHandler(Button.PointerPressedEvent, OnCenterButtonPressed, handledEventsToo: true);
        buttonCenter.AddHandler(Button.PointerReleasedEvent, OnCenterButtonReleased, handledEventsToo: true);
    }

    public byte[] GetButtons()
    {
        return new byte[]
        {
			(byte)(buttonUp.IsPressed ? 1 : 0),
			(byte)(buttonCenter.IsPressed ? 1 : 0),
			(byte)(buttonDown.IsPressed ? 1 : 0),
			(byte)(buttonRight.IsPressed ? 1 : 0),
			(byte)(buttonLeft.IsPressed ? 1 : 0),
			(byte)(buttonBack.IsPressed ? 1 : 0),
		};
    }

    private void OnCenterButtonPressed(object sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        CenterButtonPressed?.Invoke();
    }

    private void OnCenterButtonReleased(object sender, Avalonia.Input.PointerReleasedEventArgs e)
    {
        CenterButtonReleased?.Invoke();
    }
}
