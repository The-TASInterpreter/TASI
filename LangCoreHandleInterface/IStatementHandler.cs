﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.RuntimeObjects;

namespace TASI.LangCoreHandleInterface
{
    public interface IStatementHandler
    {
        Value? HandleStatement(List<Command> inputStatement, AccessableObjects accessableObjects);
    }
}
