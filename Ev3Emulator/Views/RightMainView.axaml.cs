using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Ev3Emulator.Interfaces;

namespace Ev3Emulator;

public partial class RightMainView : UserControl, IRightMainView
{
    public RightMainView()
    {
        InitializeComponent();
    }
}