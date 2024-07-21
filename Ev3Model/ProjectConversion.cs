using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EV3ModelLib
{
    public interface IProjectConverter
    {
        ProjectResultData Convert(Stream instream, string filename);
    }

    public static class ProjectConversion
    {
        private static Dictionary<string, Type> ConvertersForExtension = new Dictionary<string, Type>();
        public static void RegisterConverter(string extension, Type converterType)
        {
            if (!ConvertersForExtension.ContainsKey(extension) &&
                converterType.GetInterfaces().Contains(typeof(IProjectConverter)))
            {
                ConvertersForExtension[extension] = converterType;
            }
        }
        public static ProjectResultData Convert(Stream instream, string filename)
        {
            if (ConvertersForExtension.TryGetValue(Path.GetExtension(filename).ToLower(), out Type converterType))
            {
                var instance = (IProjectConverter)Activator.CreateInstance(converterType);
                ProjectResultData resultData = instance.Convert(instream, filename);
                return resultData;
            }
            else return null;
        }
    }
}
