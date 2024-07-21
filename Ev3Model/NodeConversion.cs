using System;
using System.Collections.Generic;
using System.Linq;

namespace EV3ModelLib
{
    public interface INodeConverter
    {
        Node Convert(object source);
    }

    public static class NodeConversion
    {
        private static Dictionary<Type, INodeConverter> Converters = new Dictionary<Type, INodeConverter>();
        public static void RegisterConverter(Type converterType)
        {
            if (!Converters.ContainsKey(converterType) &&
                converterType.GetInterfaces().Contains(typeof(INodeConverter)))
            {
                Converters[converterType] = (INodeConverter)Activator.CreateInstance(converterType);
            }
        }
        public static Node Convert(object source)
        {
            foreach (var converterKvp in Converters)
            {
                var retval = converterKvp.Value.Convert(source);
                if (retval != null) return retval;
            }
            return null;
        }

    }
}
