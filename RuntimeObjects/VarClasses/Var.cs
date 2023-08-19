using TASI.InternalLangCoreHandle;
using TASI.RuntimeObjects;
using TASI.Types.Definition;
using TASI.Types.Instance;

namespace TASI.RuntimeObjects.VarClasses
{
    public class Var
    {
        public TypeDef varType;
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
                        Value result = InterpretMain.InterpretNormalMode(command.codeContainerCommands, accessableObjects) ?? throw new CodeSyntaxException($"Promise for \"{varType.name}\" returned not the expected {varType.type}-type");
                        if (Value.ConvertValueTypeToVarType(result.valueType ?? throw new InternalInterpreterException("Value type is null")) != varType.type)
                        {
                            throw new CodeSyntaxException($"Promise for \"{varType.name}\" returned not the expected {varType.type}-type");
                        }
                        varValueHolder.value = result;
                        promised = null;
                        promiseCancel = null;
                    }
                    catch (OperationCanceledException) { return; }
                }, promiseCancel.Token);

                promised.Start();
            }

        }

        public Var(Var var, bool makeLink = false, bool makeConst = false)
        {
            varType = new(var.varType.type, var.varType.name, var.varType.isLink, var.varType.isConstant);
            if (makeLink)
                varValueHolder = var.varValueHolder;
            else
                varValueHolder = new(new(var.VarValue));
        }

        public Var(TypeDef expectedType, TypeInstance varValue)
        {
            varType = expectedType;
            if (!varValue.typeDef.CanBeUsedAs(expectedType))
                throw new CodeSyntaxException($"The type \"{varValue.typeDef.GetFullName}\" can't be used as \"{expectedType.GetFullName}\"");
            varValueHolder = new(varValue);


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
                    if (varType.type == VarConstruct.VarType.all)
                    {
                        varValueHolder.value = value;
                        return;
                    }
                    switch (varType.type)
                    {
                        case VarConstruct.VarType.num:
                            if (value.valueType != Value.ValueType.num) throw new CodeSyntaxException($"{value.valueType} is not the expected {varType.type}-type, the \"{varType.name}\" variable expects, or can't be converted to that type");
                            varValueHolder.value = value;
                            break;
                        case VarConstruct.VarType.@int:
                            if (value.valueType != Value.ValueType.@int) throw new CodeSyntaxException($"{value.valueType} is not the expected {varType.type}-type, the \"{varType.name}\" variable expects, or can't be converted to that type");
                            varValueHolder.value = value;

                            break;
                        case VarConstruct.VarType.@bool:
                            if (varType.type != VarConstruct.VarType.@bool) throw new CodeSyntaxException($"{value.valueType} is not the expected {varType.type}-type, the \"{varType.name}\" variable expects");
                            varValueHolder.value = value;
                            break;
                        case VarConstruct.VarType.@string:
                            if (varType.type != VarConstruct.VarType.@string) throw new CodeSyntaxException($"{value.valueType} is not the expected {varType.type}-type, the \"{varType.name}\" variable expects");
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
