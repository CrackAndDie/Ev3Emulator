using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EV3DecompilerLib.BuildEV3
{
    public class CreateProjectTitle : IEV3GZipBlock
    {
        public string Name { get { return "___ProjectTitle"; } }
        public string GetContent()
        {
            return "Generated Project";
        }
    }
}
