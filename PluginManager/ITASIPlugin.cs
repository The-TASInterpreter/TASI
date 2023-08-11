using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI.PluginManager
{
    public interface ITASIPlugin
    {
        string PluginName { get; }
        string PluginVersion { get; }
        string PluginDescription { get; }

        string Copyright { get; }

    }
}
