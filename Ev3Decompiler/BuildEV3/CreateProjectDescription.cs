using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EV3DecompilerLib.BuildEV3
{
    public class CreateProjectDescription : IEV3GZipBlock
    {
        public string ProjectDescription { get; set; }
        public string Name { get { return "___ProjectDescription"; } }
        public string GetContent()
        {
            return $"{ProjectDescription}\n\nEV3MagicTools: EV3TreeVisualizer and EV3BrickMagic online at http://ev3treevis.azurewebsites.net\n\n{DateTime.Now.ToString()}";
        }
    }
}
