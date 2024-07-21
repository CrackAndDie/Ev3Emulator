using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace EV3DecompilerLib.BuildEV3
{
    class CreateProgramFromXml : IEV3GZipBlock
    {
        private readonly string data;
        private readonly string sProgName;

        public CreateProgramFromXml(byte[] data, string sProgName)
        {
            var ms = new MemoryStream(data);
            var xdoc = XDocument.Load(ms);
            xdoc.Declaration = new XDeclaration("1.0", "utf-8", null);
            using (var sw = new Utf8StringWriter())
            {
                xdoc.Save(sw);
                this.data = sw.ToString();
            }
            this.sProgName = sProgName;
        }

        public string Name { get { return FileNameConverter(sProgName); } }
        public static string FileNameConverter(string fname)
        {
            return Path.GetFileNameWithoutExtension(fname) + ".ev3p";
        }

        string IEV3GZipBlock.GetContent()
        {
            return data;
        }
    }

    public sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }

    class CreateMyBlockXmlFromXml : IEV3GZipBlock
    {
        private readonly string data;
        private readonly string sProgName;

        public CreateMyBlockXmlFromXml(byte[] data, string sProgName)
        {
            var ms = new MemoryStream(data);
            var xdoc = XDocument.Load(ms);
            xdoc.Declaration = new XDeclaration("1.0", "utf-8", null);
            using (var sw = new Utf8StringWriter())
            {
                xdoc.Save(sw);
                this.data = sw.ToString();
            }
            this.sProgName = sProgName;
        }

        public string Name { get { return FileNameConverter(sProgName); } }
        public static string FileNameConverter(string fname)
        {
            return Path.GetFileNameWithoutExtension(fname) + ".mbxml";
        }

        string IEV3GZipBlock.GetContent()
        {
            return data;
        }
    }
}
