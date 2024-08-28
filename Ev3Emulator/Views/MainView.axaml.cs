using Avalonia.Controls;
using Ev3Emulator.Interfaces;

namespace Ev3Emulator.Views;

public partial class MainView : UserControl, IMainView
{
    public MainView()
    {
        InitializeComponent();
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
}
