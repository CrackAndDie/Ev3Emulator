using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Ev3CoreUnsafe;
using Ev3Emulator.CoreImpl;
using Hypocrite.Core.Mvvm.Attributes;
using Hypocrite.Mvvm;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;

namespace Ev3Emulator.ViewModels;

public class MainViewModel : ViewModelBase
{
	public MainViewModel()
	{
		StartCommand = new DelegateCommand(OnStartCommand);
	}

	public override void OnViewReady()
	{
		base.OnViewReady();

		LcdBitmap = new WriteableBitmap(new PixelSize(Defines.vmLCD_WIDTH, Defines.vmLCD_HEIGHT), new Vector(96, 96), Avalonia.Platform.PixelFormat.Rgba8888);
		(GH.Ev3System.LcdHandler as LcdHandler).Bitmap = LcdBitmap;

		(GH.Ev3System.Logger as ViewLogger).LogAction = UpdateLog;
	}

	private void UpdateLog(LogData data)
	{
		Dispatcher.UIThread.Invoke(() =>
		{
			OutputData.Add(data);
			LastData = data;
		});
	}

	private void OnStartCommand()
	{
		_ev3Thread = new Thread(GH.Main);
		_ev3Thread.Start();
	}

	[Notify]
	public ICommand StartCommand { get; set; }

	public ObservableCollection<LogData> OutputData { get; } = new ObservableCollection<LogData>();

	[Notify]
    public WriteableBitmap LcdBitmap { get; set; }
	[Notify]
	public LogData LastData { get; set; }

	private Thread _ev3Thread;
}
