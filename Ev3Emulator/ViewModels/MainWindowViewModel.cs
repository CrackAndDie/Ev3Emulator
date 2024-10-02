using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Ev3LowLevelLib;
using Hypocrite.Core.Mvvm.Attributes;
using Hypocrite.Mvvm;
using Prism.Commands;
using Prism.Events;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Ev3Emulator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            OpenGitHubCommand = new DelegateCommand(OnOpenGitHubCommand);
		}
		private void OnOpenGitHubCommand()
        {
            string link = "https://github.com/CrackAndDie/Ev3Emulator";
			// TODO: use launcher or custom for each platform
			Process.Start(new ProcessStartInfo("cmd", $"/c start {link}") { CreateNoWindow = true });
		}

        [Notify]
        public ICommand OpenGitHubCommand { get; set; }
	}
}
