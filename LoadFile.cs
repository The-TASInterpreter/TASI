

namespace TASI
{
    internal class LoadFile
    {
        public static List<UnspecifiedMethod> LoadMethods(List<Command> fileCommands, NamespaceInfo currentNamespace)
        {
            bool currentMethod = false;
            List<Command> methodDefCommand = new(); //This stores the a method statement, so it can be processed in whole.
            List<UnspecifiedMethod> result = new();
            for (int i = 0; i < fileCommands.Count; i++)
            {
                //Im sorry for this, but it just checks, weather the current command is a method statement and is a new command or sth.
                if ((fileCommands[i - 1].commandType == Command.CommandTypes.MethodCall || fileCommands[i - 1].commandType == Command.CommandTypes.EndCommand) && fileCommands[i].commandType == Command.CommandTypes.Statement && fileCommands[i].commandText.ToLower() == "method")
                    currentMethod = true;

                if (currentMethod)
                {
                    if (fileCommands[i].commandType == Command.CommandTypes.EndCommand)
                    {
                        currentMethod = false;
                        result.Add(InterpretMethodStatement(methodDefCommand));
                        methodDefCommand = new();
                        continue;
                    }
                    methodDefCommand.Add(fileCommands[i]);
                }

            }
            return result;

        }

        private static UnspecifiedMethod InterpretMethodStatement(List<Command> commands)
        {
            //Check if statement usage is correct.
            if (commands.Count != 3) throw new Exception("Invalid method statement formating. Correct one:\nmethod <method name as statement> <code container>;");
            if (commands[0].commandType != Command.CommandTypes.Statement || commands[0].commandText != "method") throw new Exception("Internal: Usage of InterpretMethodStatement. This method only supports method statements.");
            if (commands[1].commandType != Command.CommandTypes.Statement) throw new Exception("Invalid method statement formating. Correct one:\nmethod <method name as statement> <code container>;");
            if (commands[2].commandType != Command.CommandTypes.CodeContainer) throw new Exception("Invalid method statement formating. Correct one:\nmethod <method name as statement> <code container>;");
            return new UnspecifiedMethod(commands[1].commandText, StringProcess.ConvertLineToCommand(commands[2].commandText));
        }

    }
}
