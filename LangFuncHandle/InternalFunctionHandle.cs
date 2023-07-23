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
                    FileMode mode = FileMode.Open;
                    FileAccess access = FileAccess.ReadWrite;

                    
                    if (input[1].StringValue.Contains('w'))
                        access |= FileAccess.Write;

                    if (input[1].StringValue.Contains('r'))
                        access |= FileAccess.Read;

                    if (input[1].StringValue.Contains('a'))
                        mode = FileMode.Append;

                    if (input[1].StringValue.Contains("+!"))
                        mode = FileMode.CreateNew;

                    else if (input[1].StringValue.Contains('+'))
                        mode = FileMode.Create;


                    if (input[1].StringValue.Contains("~"))
                        mode = FileMode.Truncate;

                    if (input[1].StringValue.Contains('?'))
                        mode = FileMode.OpenOrCreate;


                    FileStream stream = File.Open(input[0].StringValue, mode, access);
                    accessableObjects.global.AllFileStreams.Add(stream);

                    int streamIndex = accessableObjects.global.AllFileStreams.IndexOf(stream);

                    return new(Value.ValueType.@int, streamIndex);
                case "filesystem.close":
                    {
                
                        FileStream fileStream = accessableObjects.global.AllFileStreams[(int)input[0].NumValue];

                        fileStream.Close();
                        accessableObjects.global.AllFileStreams.RemoveAt((int)input[0].NumValue);

                        return null;
                    }
                case "filesystem.delete":
                    File.Delete(input[0].StringValue);
                    return null;
                case "filesystem.create":
                    {
                        FileStream new_stream = File.Create(input[0].StringValue);
                        accessableObjects.global.AllFileStreams.Add(new_stream);

                        int new_streamIndex = accessableObjects.global.AllFileStreams.IndexOf(new_stream);

                        return new(Value.ValueType.@int, new_streamIndex);
                    }
                case "filesystem.exists":
                    return new(Value.ValueType.@bool, File.Exists(input[0].StringValue));
                case "filestream.readline":
                    {
                        FileStream fileStream = accessableObjects.global.AllFileStreams[(int)input[0].NumValue];

                        if (!fileStream.CanRead)

                            throw new RuntimeCodeExecutionFailException("Tried to read from a stream that dosen't allow reading!", "InternalFuncException");

                        using StreamReader reader = new(fileStream);
                        string line = reader.ReadLine() ?? throw new RuntimeCodeExecutionFailException("Stream.ReadLine returned null", "InternalFuncException");

                        return new Value(Value.ValueType.@string, line);

                    }
                case "filestream.write":
                    {
                        FileStream fileStream = accessableObjects.global.AllFileStreams[(int)input[0].NumValue];

                        if (!fileStream.CanWrite)
                            throw new RuntimeCodeExecutionFailException("Tried to read from a stream that doesn't allow writing!", "InternalFuncException");

                        using StreamWriter writer = new(fileStream);
                        writer.Write((char)(int)input[1].NumValue);

                        return null;
                    }
                case "filestream.writeline":
                    {
                        
                        FileStream fileStream = accessableObjects.global.AllFileStreams[(int)input[0].NumValue];

                        if (!fileStream.CanWrite)
                            throw new RuntimeCodeExecutionFailException("Tried to read from a stream that doesn't allow writing!", "InternalFuncException");

                        using StreamWriter writer = new(fileStream);

                        if (input[1].IsNumeric)
                            writer.WriteLine(input[1].NumValue);
                        else
                            writer.WriteLine(input[1].StringValue);
                            
                        return null;
                        
                    }
                case "filestream.flush":
                    {
                        FileStream fileStream = accessableObjects.global.AllFileStreams[(int)input[0].NumValue];

                        fileStream.Flush();

                        return null;
                     
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

                case "random.next":
                    if (input.Count == 0)
                        return new(Value.ValueType.@int, accessableObjects.global.RandomGenerator.Next());
                    else if (input.Count == 1 && input[0].valueType == Value.ValueType.@int)
                        return new(Value.ValueType.@int, accessableObjects.global.RandomGenerator.Next((int)input[0].NumValue));
                    else if (input.Count == 2 && input[0].valueType == Value.ValueType.@int && input[1].valueType == Value.ValueType.@int)
                        return new(Value.ValueType.@int, accessableObjects.global.RandomGenerator.Next((int)input[0].NumValue, (int)input[1].NumValue));

                    throw new CodeSyntaxException("Invalid usage of the \"Random.Next\" function. Correct usage: Random.Next [<int: min>] [<int: max>];");

                case "random.nextnum":
                    if (input.Count == 0)
                        return new(Value.ValueType.num, accessableObjects.global.RandomGenerator.NextDouble());

                    throw new CodeSyntaxException("Invalid usage of the \"Random.Next\" function. It dosn't take any paramters!");

                default: throw new InternalInterpreterException("Internal: No definition for " + funcName);
            }

        }




    }

}

