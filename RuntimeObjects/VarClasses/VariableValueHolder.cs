using TASI.RuntimeObjects;
using TASI.Types.Instance;

namespace TASI.RuntimeObjects.VarClasses
{
    public class VariableValueHolder
    {
        public bool waitingPromise = false;
        public TypeInstance value;
        public VariableValueHolder(TypeInstance value) { this.value = value; }
    }
}
