using Avalonia.Controls;
using Ev3Emulator.Events;
using Hypocrite.Core.Mvvm.Attributes;
using Hypocrite.Mvvm;
using Prism.Commands;
using Prism.Events;
using System.Windows.Input;

namespace Ev3Emulator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            CloseCommand = new DelegateCommand<Window>(OnCloseCommand);
        }

        private void OnCloseCommand(Window window)
        {
            EventAggregator.GetEvent<AppCloseEvent>().Publish();
            window.Close();
		}

        [Notify]
        public ICommand CloseCommand { get; set; }
    }
}
