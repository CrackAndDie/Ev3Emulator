using Hypocrite.Core.Container.Interfaces;
using Prism.Ioc;
using System;

namespace Ev3Emulator.Extensions
{
    public static class ContainerExtensions
    {
        public static void RegisterViewForNavigation<TFrom, TTo>(this IContainerRegistry containerRegistry)
            where TTo : class, TFrom
        {
            containerRegistry.Register<object, TTo>(typeof(TFrom).Name);
            containerRegistry.Register<TFrom, TTo>(typeof(TFrom).Name);
        }

        public static void RegisterSingletonViewForNavigation<TFrom, TTo>(this IContainerRegistry containerRegistry, Type viewModelType)
            where TTo : class, TFrom
        {
            containerRegistry.RegisterSingleton<object, TTo>(typeof(TFrom).Name);
            containerRegistry.RegisterSingleton<TFrom, TTo>(typeof(TFrom).Name);
            containerRegistry.RegisterSingleton(viewModelType);
        }

        public static ILightContainer RegisterInstance<TInterface>(this ILightContainer container, TInterface instance)
        {
            return (container ?? throw new ArgumentNullException(nameof(container)))
                .RegisterInstance(typeof(TInterface), instance);
        }
    }
}
