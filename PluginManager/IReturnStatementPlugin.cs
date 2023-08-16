using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.LangCoreHandleInterface;

namespace TASI.PluginManager
{
    public interface IReturnStatementPlugin : ITASIPlugin, IReturnStatementHandler
    {
        void InitStatements(Global global);
    }
}
