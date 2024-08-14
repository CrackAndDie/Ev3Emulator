using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;

using Ev3Emulator.ViewModels;
using Ev3Emulator.Views;
using Hypocrite.Core.Interfaces.Presentation;
using Hypocrite.Core.Interfaces;
using Hypocrite.Core.Logging.Interfaces;
using Hypocrite.Core.Logging.Services;
using Hypocrite.Core.Services;
using Hypocrite.Core.Utils.Settings;
using Hypocrite.Localization;
using Hypocrite.Mvvm;
using Hypocrite.Services;
using Microsoft.VisualBasic;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.IO;
using static Avalonia.AvaloniaLocator;
using System.Reflection;
using System.Threading;
using Ev3Emulator.Modules;
using Ev3CoreUnsafe;
using Ev3Emulator.CoreImpl;

namespace Ev3Emulator;

public partial class App : ApplicationBase
{
    private static readonly string logFileFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SoftHubSettings");
    private static readonly string logFileName = "Ev3EmulatorAvalonia.log";

    public override void Initialize()
    {
		// init ev3
		GH.Ev3System = new Ev3System();

		Thread.CurrentThread.Name = "MainThread";
        AvaloniaXamlLoader.Load(this);
        base.Initialize();              // <-- Required
    }

    protected override AvaloniaObject CreateShell()
    {
        var viewModelService = Container.Resolve<IViewModelResolverService>();
        viewModelService.RegisterViewModelAssembly(Assembly.GetExecutingAssembly());

        return base.CreateShell();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        base.RegisterTypes(containerRegistry);

        CheckForLogPathExistance();
        containerRegistry.RegisterInstance<ILoggingService>(new Log4netLoggingService(Path.Combine(logFileFolder, logFileName)));
        containerRegistry.RegisterSingleton<IViewModelResolverService, ViewModelResolverService>();
        containerRegistry.RegisterSingleton<IWindowProgressService, WindowProgressService>();

        // registering backend services
        // RegistrationHelper.RegisterBackendServices(containerRegistry);

        RegisterAppServices(containerRegistry);
    }

    private void RegisterAppServices(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IBaseWindow, MainWindow>();
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        base.ConfigureModuleCatalog(moduleCatalog);
        moduleCatalog.AddModule<MainModule>();
    }

    private void CheckForLogPathExistance()
    {
        if (!Directory.Exists(logFileFolder))
        {
            Directory.CreateDirectory(logFileFolder);
        }
    }
}
