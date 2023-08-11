using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI.PluginManager
{
    /// <summary>
    /// This plugin will run before any code is executed but after all files were parsed
    /// </summary>
    public interface InitialisationPlugin : ITASIPlugin
    {
        void Execute(AccessableObjects accessableObjects, List<Command> commands);
    }
}
