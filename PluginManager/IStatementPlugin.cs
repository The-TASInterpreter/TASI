﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.LangCoreHandleInterface;

namespace TASI.PluginManager
{
    public interface IStatementPlugin : ITASIPlugin, IStatementHandler
    {
        void InitStatements(Global global);

    }
}
