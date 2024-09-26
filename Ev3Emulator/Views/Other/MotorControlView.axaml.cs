using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Ev3Emulator.Interfaces;
using Hypocrite.Core.Interfaces.Presentation;

namespace Ev3Emulator.Views.Other;

public partial class MotorControlView : UserControl, IMotorControlView
{
    public MotorControlView()
    {
        InitializeComponent();
    }
}