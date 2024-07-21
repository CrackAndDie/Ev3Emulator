using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EV3DecompilerLib.BuildEV3
{
    public static class XNameSpaceExtensions
    {
        //, new XAttribute("xmlns1", xmlns_sm) //XNamespace.Xmlns
        public static void SetDefaultXmlNamespace(this XElement xelem, XNamespace xmlns)
        {
            if (xelem.Name.NamespaceName == string.Empty)
                xelem.Name = xmlns + xelem.Name.LocalName;
            foreach (var e in xelem.Elements())
                e.SetDefaultXmlNamespace(xmlns);
        }
        public static XElement AddAttributes(this XElement xelem, Dictionary<string, string> attribute_kvps)
        {
            foreach (var kvp in attribute_kvps)
            {
                xelem.Add(new XAttribute(kvp.Key, kvp.Value));
            }
            return xelem;
        }
    }

    public static class XExt
    {
        public static XElement CreateXElementWithNS(XName name, XNamespace xns)
        {
            var retval = new XElement(name);
            retval.SetDefaultXmlNamespace(xns);
            return retval;
        }
        public static XElement CreateXElementWithNS(XName name, XNamespace xns, params object[] content)
        {
            var retval = new XElement(name, content);
            retval.SetDefaultXmlNamespace(xns);
            return retval;
        }
    }
}
