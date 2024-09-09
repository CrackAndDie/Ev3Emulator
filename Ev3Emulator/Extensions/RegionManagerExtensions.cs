using Avalonia;
using Hypocrite.Core.Logging.Interfaces;
using Hypocrite.Mvvm;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Ev3Emulator.Extensions
{
	public static class RegionManagerExtensions
	{
		public static void ReqNav(this IRegionManager regionManager, Type view, string regionName)
		{
			regionManager.RequestNavigate(regionName, view.Name, OnNavigationFinished);
		}

		public static void ReqNav(this IRegionManager regionManager, Type view, string regionName, object parameters)
		{
			ConstructorInfo constructor = typeof(GenericNavigationParameters<>).MakeGenericType(parameters.GetType()).GetConstructor(new Type[1] { parameters.GetType() });
			regionManager.RequestNavigate(regionName, view.Name, OnNavigationFinished, constructor.Invoke(new object[1] { parameters }) as NavigationParameters);
		}

		private static void OnNavigationFinished(NavigationResult result)
		{
			if (result.Error != null)
			{
				ILoggingService loggingService = (Application.Current as ApplicationBase).Container.Resolve<ILoggingService>();
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Navigation failed! Target: ");
				defaultInterpolatedStringHandler.AppendFormatted(result.Context.Uri);
				loggingService.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				throw result.Error;
			}
		}
	}
}
