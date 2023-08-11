using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI.LangFuncHandle
{
    public interface IInternalFunctionHandle
    {
        Value? HandleInternalFunc(string funcName, List<Value> input, AccessableObjects accessableObjects);
    }
}
