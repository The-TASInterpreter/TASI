namespace TASI.Objects.VarClasses
{
    public class VariableValueHolder
    {
        public bool waitingPromise = false;
        public Value value;
        public VariableValueHolder(Value value) { this.value = value; }
    }
}
