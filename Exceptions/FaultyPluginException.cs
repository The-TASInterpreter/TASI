using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.PluginManager;

namespace TASI
{
    public class FaultyPluginException : Exception
    {
        public ITASIPlugin faultyPlugin;
        /// <summary>
        /// This should get thrown if a plugin could not be loaded because it is faulty
        /// </summary>
        /// <param Name="message"></param>
        /// <param Name="faultyPlugin"></param>
        internal FaultyPluginException(string message, ITASIPlugin faultyPlugin) : base(message)
        {
            this.faultyPlugin = faultyPlugin;
        }
    }
}
