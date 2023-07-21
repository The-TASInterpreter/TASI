using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters;

namespace TASI
{
    internal class InternalFunctionHandle
    {

        public static Value? HandleInternalFunc(string funcName, List<Value> input, AccessableObjects accessableObjects)
        {
            switch (funcName)
            {
                case "test.helloworld":
                    if (input[0].NumValue == 1)
                        Console.WriteLine(input[1].StringValue);
                    else
                        Console.WriteLine("No text pritable.");
                    return null;
                case "console.readline":
                    return new(Value.ValueType.@string, Console.ReadLine()?? throw new RuntimeCodeExecutionFailException("Console.ReadLine returned null", "InternalFuncException"));
                case "console.clear":
                    Console.Clear();
                    return null;
                case "console.writeline":

                    if (input[0].IsNumeric)
                        Console.WriteLine(input[0].NumValue);
                    else
                        Console.WriteLine(input[0].StringValue);
                    return null;
                case "filesystem.open":
                    FileStream stream = File.Open(input[0].StringValue, FileMode.Open);
                    accessableObjects.global.AllFileStreams.Add(stream);

                    int streamIndex = accessableObjects.global.AllFileStreams.IndexOf(stream);

                    return new(Value.ValueType.@int, streamIndex);
                case "filesystem.close":
                    accessableObjects.global.AllFileStreams[(int)input[0].NumValue].Close();

                    return null;
                case "filestream.readline":
                    {
                        FileStream fileStream = accessableObjects.global.AllFileStreams[(int)input[0].NumValue];

                        if (!fileStream.CanRead)
                            throw new RuntimeCodeExecutionFailException("Tried to read from a stream that dosen't allow reading!", "InternalFuncException");

                        using StreamReader reader = new(fileStream);
                        string line = reader.ReadLine() ?? throw new RuntimeCodeExecutionFailException("Stream.ReadLine returned null", "InternalFuncException");

                        return new Value(Value.ValueType.@string, line);

                    }
                case "filestream.read":
                    {
                        FileStream fileStream = accessableObjects.global.AllFileStreams[(int)input[0].NumValue];

                        if (!fileStream.CanRead)
                            throw new RuntimeCodeExecutionFailException("Tried to read from a stream that dosen't allow reading!", "InternalFuncException");

                        using StreamReader reader = new(fileStream);
                        int character = reader.Read();

                        return new Value(Value.ValueType.@int, character);

                    }
                case "program.pause":
                    if (input.Count == 1 && input[0].NumValue == 1)
                        Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    return null;
                case "inf.defvar":

                    if (!Enum.TryParse<Value.ValueType>(input[0].StringValue, true, out Value.ValueType varType) && input[0].StringValue != "all") throw new CodeSyntaxException($"The vartype \"{input[0].StringValue}\" doesn't exist.");
                    if (input[0].StringValue == "all")
                    {
                        accessableObjects.accessableVars.Add(input[1].StringValue, new Var(new VarConstruct(VarConstruct.VarType.all, input[1].StringValue), new(varType)));
                        return null;
                    }
                    accessableObjects.accessableVars.Add(input[1].StringValue, new Var(new VarConstruct(Value.ConvertValueTypeToVarType(varType), input[1].StringValue), new(varType)));
                    return null;
                case "convert.tonum":
                    if (!double.TryParse(input[0].StringValue, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double result))
                        if (input[1].BoolValue)
                            throw new CodeSyntaxException("Can't convert string in current format to double.");
                        else
                            return new(Value.ValueType.num, double.NaN);



                    return new(Value.ValueType.num, result);



                default: throw new InternalInterpreterException("Internal: No definition for " + funcName);
            }

        }




    }

}

