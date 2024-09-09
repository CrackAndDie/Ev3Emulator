using Avalonia.Controls;
using Hypocrite.Core.Interfaces.Presentation;

namespace Ev3Emulator.Views;

public partial class MainWindow : Window, IBaseWindow
{
    public MainWindow()
    {
        InitializeComponent();

		this.Get<Border>("WindowHeader").PointerPressed += (i, e) =>
		{
			BeginMoveDrag(e);
		};
	}
}
