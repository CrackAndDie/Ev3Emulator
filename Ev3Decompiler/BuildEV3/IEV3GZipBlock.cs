using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EV3DecompilerLib.BuildEV3
{
    interface IEV3GZipBlock
    {
        string Name { get; }
        string GetContent();
    }
}
