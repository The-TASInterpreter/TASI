using TASI.Objects.VarClasses;

namespace TASI
{
    public class Var
    {
        public VarConstruct varConstruct;
        public VariableValueHolder varValueHolder;
        public Task? promised = null;
        public CancellationTokenSource? promiseCancel;
        public object taskLock = new();

        public void CancelPromise()
        {
            promiseCancel.Cancel();



            WaitPromise();

            promised = null;
            promiseCancel = null;
        }

        public void WaitPromise()
        {
            if (promised != null)
            {
                    promised.Wait();


            }
        }

        public void Promise(Command command, AccessableObjects accessableObjects)
        {

            lock (taskLock)
            {

                promiseCancel = new();
                accessableObjects.cancellationTokenSource = promiseCancel;
                promised = new(() =>
                {

                    try
                    {
                        Value result = InterpretMain.InterpretNormalMode(command.codeContainerCommands, accessableObjects) ?? throw new CodeSyntaxException($"Promise for \"{varConstruct.name}\" returned not the expected {varConstruct.type}-type");
                        if (Value.ConvertValueTypeToVarType(result.valueType ?? throw new InternalInterpreterException("Value type is null")) != varConstruct.type)
                        {
                            throw new CodeSyntaxException($"Promise for \"{varConstruct.name}\" returned not the expected {varConstruct.type}-type");
                        }
                        varValueHolder.value = result;
                        promised = null;
                        promiseCancel = null;
                    } catch (OperationCanceledException) { return; }
                }, promiseCancel.Token);

                promised.Start();
            }

        }

        public Var(Var var, bool makeLink = false)
        {
            varConstruct = new(var.varConstruct.type, var.varConstruct.name, var.varConstruct.isLink);
            if (makeLink)
                varValueHolder = var.varValueHolder;
            else
                varValueHolder = new(new(var.VarValue));
        }

        public Var(VarConstruct varConstruct, Value varValue)
        {
            this.varConstruct = varConstruct;



            this.varValueHolder = new(varValue);

            if (Value.ConvertValueTypeToVarType(VarValue.valueType ?? throw new InternalInterpreterException("Value type of value was null")) != varConstruct.type && varConstruct.type != VarConstruct.VarType.all) throw new CodeSyntaxException($"The variable \"{varConstruct.name}\" can't be initialized with a {varValue.valueType}-type value, because it's a {varConstruct.type}-type variable.");

        }

        public Value VarValue
        {
            get
            {
                lock (taskLock)
                {
                    WaitPromise();
                    return varValueHolder.value;
                }
            }
            set
            {
                lock (taskLock)
                {
                    if (promised != null)
                    {
                        CancelPromise();
                    }
                    if (varConstruct.type == VarConstruct.VarType.all)
                    {
                        varValueHolder.value = value;
                        return;
                    }
                    switch (value.valueType)
                    {
                        case Value.ValueType.num:
                            if (varConstruct.type != VarConstruct.VarType.num) throw new CodeSyntaxException($"{value.valueType} is not the expected {varConstruct.type}-type, the \"{varConstruct.name}\" variable expects");
                            varValueHolder.value = value;
                            break;
                        case Value.ValueType.@bool:
                            if (varConstruct.type != VarConstruct.VarType.@bool) throw new CodeSyntaxException($"{value.valueType} is not the expected {varConstruct.type}-type, the \"{varConstruct.name}\" variable expects");
                            varValueHolder.value = value;
                            break;
                        case Value.ValueType.@string:
                            if (varConstruct.type != VarConstruct.VarType.@string) throw new CodeSyntaxException($"{value.valueType} is not the expected {varConstruct.type}-type, the \"{varConstruct.name}\" variable expects");
                            varValueHolder.value = value;
                            break;
                        default:
                            throw new InternalInterpreterException($"Internal: The {value.valueType}-type is not implemented.");
                    }
                }
            }


        }


    }
}
