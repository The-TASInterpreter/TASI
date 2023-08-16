using TASI.RuntimeObjects;

namespace TASI.RuntimeObjects.VarClasses
{
    public class VariableValueHolder
    {
        public bool waitingPromise = false;
        public Value value;
        public VariableValueHolder(Value value) { this.value = value; }
    }
}
