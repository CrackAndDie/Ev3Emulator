using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Ev3CoreUnsafe;
using Ev3CoreUnsafe.Helpers;
using Ev3Emulator.CoreImpl;
using Hypocrite.Core.Mvvm.Attributes;
using Hypocrite.Mvvm;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

		if (Design.IsDesignMode)
			return;

		(GH.Ev3System.LcdHandler as LcdHandler).BitmapAction = UpdateLcd;
		(GH.Ev3System.Logger as ViewLogger).LogAction = UpdateLog;

		var aniBmp = BmpHelper.Get(Ev3CoreUnsafe.Enums.BmpType.Ani1x);
		var aniBitmap = new WriteableBitmap(new PixelSize(aniBmp.Width, aniBmp.Height), new Vector(96, 96), Avalonia.Platform.PixelFormat.Rgba8888);
		using (var frameBuffer = aniBitmap.Lock())
		{
			// * 4 because orig data is grayscale
			Marshal.Copy(LcdHandler.ConvertToRgba8888(aniBmp.Data), 0, frameBuffer.Address, aniBmp.Data.Length * 4);
		}
		aniBitmap.Save("ani1.bmp", 100);
		var mind = BmpHelper.Get(Ev3CoreUnsafe.Enums.BmpType.Mindstorms);
		var mindBitmap = new WriteableBitmap(new PixelSize(mind.Width, mind.Height), new Vector(96, 96), Avalonia.Platform.PixelFormat.Rgba8888);
		using (var frameBuffer = mindBitmap.Lock())
		{
			// * 4 because orig data is grayscale
			Marshal.Copy(LcdHandler.ConvertToRgba8888(mind.Data), 0, frameBuffer.Address, mind.Data.Length * 4);
		}
		mindBitmap.Save("amind1.bmp", 100);

		var fnt = BmpHelper.Get(Ev3CoreUnsafe.Enums.BmpType.SmallFont);
		var fntBitmap = new WriteableBitmap(new PixelSize(fnt.Width, fnt.Height), new Vector(96, 96), Avalonia.Platform.PixelFormat.Rgba8888);
		using (var frameBuffer = fntBitmap.Lock())
		{
			// * 4 because orig data is grayscale
			Marshal.Copy(LcdHandler.ConvertToRgba8888(fnt.Data), 0, frameBuffer.Address, fnt.Data.Length * 4);
		}
		fntBitmap.Save("afnt.bmp", 100);
	}

	private void UpdateLog(LogData data)
	{
		Dispatcher.UIThread.Invoke(() =>
		{
			OutputData.Add(data);
			LastData = data;
		});
	}

	private void UpdateLcd(Bitmap bmp)
	{
        Dispatcher.UIThread.Invoke(() =>
        {
            LcdBitmap = bmp;
			bmp.Save("Anime.bmp", 100);
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
    public Bitmap LcdBitmap { get; set; }
	[Notify]
	public LogData LastData { get; set; }

	private Thread _ev3Thread;
}
