using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.LangFuncHandle;

namespace TASI.PluginManager
{
    public interface IInternalFunctionPlugin : ITASIPlugin, IInternalFunctionHandler
    {
        void InitFunctions(Global global);
    }
}
