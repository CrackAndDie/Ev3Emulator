﻿using Avalonia;
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
using System.Reflection;
using System.Threading;
using Ev3Emulator.Modules;
using Ev3LowLevelLib;
using Prism.Events;
using Ev3Emulator.Events;
using Avalonia.Controls;

namespace Ev3Emulator;

public partial class App : ApplicationBase
{
    private static readonly string logFileFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SoftHubSettings");
    private static readonly string logFileName = "Ev3EmulatorAvalonia.log";

    public override void Initialize()
    {
		Thread.CurrentThread.Name = "MainThread";
        AvaloniaXamlLoader.Load(this);
        base.Initialize();              // <-- Required
    }

    public override void OnFrameworkInitializationCompleted()
    {
		base.OnFrameworkInitializationCompleted();

		if (Design.IsDesignMode)
			return;

		Container.Resolve<IEventAggregator>().GetEvent<AppCloseEvent>().Subscribe(OnApplicationExit);
    }

    protected override AvaloniaObject CreateShell()
    {
		if (Design.IsDesignMode)
			return null;

		var viewModelService = Container.Resolve<IViewModelResolverService>();
        viewModelService.RegisterViewModelAssembly(Assembly.GetExecutingAssembly());

		return base.CreateShell();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        base.RegisterTypes(containerRegistry);

		if (Design.IsDesignMode)
			return;

		CheckForLogPathExistance();
        containerRegistry.RegisterInstance<ILoggingService>(new Log4netLoggingService(Path.Combine(logFileFolder, logFileName)));
        containerRegistry.RegisterSingleton<IViewModelResolverService, ViewModelResolverService>();
        containerRegistry.RegisterSingleton<IWindowProgressService, WindowProgressService>();

        containerRegistry.RegisterSingleton<Ev3Entity, Ev3Entity>();

        RegisterAppServices(containerRegistry);
    }

    private void RegisterAppServices(IContainerRegistry containerRegistry)
    {
		if (Design.IsDesignMode)
			return;

		containerRegistry.RegisterSingleton<IBaseWindow, MainWindow>();
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
		if (Design.IsDesignMode)
			return;

		base.ConfigureModuleCatalog(moduleCatalog);
        moduleCatalog.AddModule<MainModule>();
    }

    private void OnApplicationExit()
    {
		if (Design.IsDesignMode)
			return;

		Container.Resolve<Ev3Entity>().StopVm();
    }

    private void CheckForLogPathExistance()
    {
        if (!Directory.Exists(logFileFolder))
        {
            Directory.CreateDirectory(logFileFolder);
        }
    }
}
