using Ev3Emulator.Extensions;
using Ev3Emulator.Interfaces;
using Ev3Emulator.Views;
using Ev3Emulator.Views.Other;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Ev3Emulator.Modules
{
    internal class MainModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var region = containerProvider.Resolve<IRegionManager>();
            // the view to display on start up
            region.RegisterViewWithRegion(Regions.MAIN_REGION, typeof(IMainView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterViewForNavigation<IMainView, MainView>();
            containerRegistry.RegisterViewForNavigation<IRightMainView, RightMainView>();

			// control views
			containerRegistry.RegisterViewForNavigation<IMotorControlView, MotorControlView>();
		}
    }
}
