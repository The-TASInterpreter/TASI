using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.InternalLangCoreHandle;
using TASI.RuntimeObjects;

namespace TASI.LangCoreHandleInterface
{
    public interface IInternalFunctionHandler
    {
        /// <summary>
        /// This is called if a Function with an Internal-type namespace and this Handler is called. You can find an example implementation here: <see cref="InternalFunctionHandler"/>
        /// </summary>
        /// <param name="funcName">namespace.functionName e.g. conslole.writeLine. Please keep in that casing is removed.</param>
        /// <param name="input">the input values. The amount and type that were specified by the function have already been verified</param>
        /// <param name="accessableObjects"></param>
        /// <returns></returns>
        Value? HandleInternalFunc(string funcName, List<Value> input, AccessableObjects accessableObjects);
    }
}
