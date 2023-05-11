using static TASI.Command;

namespace TASI
{
    internal class LoadFile
    {
        public static List<Command> ByPath(string location, bool autoAddToGlobal = true)
        {
            location = location.Trim('"');
            if (!File.Exists(location)) throw new CodeSyntaxException("The entered file doesn't exist.");
            List<string> codeFile = File.ReadAllLines(location).ToList();
            for (int i = 0; i < codeFile.Count; i++)
            {
                string lineWithoutCommands = "";
                for (int j = 0; j < codeFile[i].Length; j++)
                {
                    if ((codeFile[i][j] == '#' && j == 0) || (codeFile[i][j] == '#' && codeFile[i][j - 1] != '\\')) break; //Remove what comes next in the line, if there is a comment
                    lineWithoutCommands += codeFile[i][j];
                }
                codeFile[i] = lineWithoutCommands;
            }

            string allFileCode = "";
            for (int i = 0; i < codeFile.Count; i++)
            {
                string line = codeFile[i];
                if (line.Contains('Ⅼ')) throw new CodeSyntaxException($"Uhhhhmmm this is a weird error now. So basically, on line {i + 1} you used a character, that is already used by TASI to map code to lines (The character is:(I would have inserted it here right now, but the console can't even print this char. It looks like an L, but it's a bit larger.)). I picked this character, because I thought noone would use it directly in their code. Well, seems like I thought wrong... Simply said, you must remove this character from your code. But fear now! With the return statement \"lineChar\", you can paste this char into strings and stuff. I hope this character is worth the errors with lines! I'm sorry.\n-Ekischleki");
                allFileCode += $"Ⅼ{i}Ⅼ{line}";
            }
            if (autoAddToGlobal)
                Global.allLoadedFiles.Add(location);
            return StringProcess.ConvertLineToCommand(allFileCode);

        }
        public static Value? RunCode(string code)
        {
            List<Command> tokenisedCode = StringProcess.ConvertLineToCommand(code);
            var codeHeaderInformation = InterpretMain.InterpretHeaders(tokenisedCode, "");
            AccessableObjects initialAccessableObjects = new(new(), codeHeaderInformation.Item2);

            foreach (NamespaceInfo namespaceInfo in Global.Namespaces) //Activate functioncalls after scanning headers to not cause any errors. BTW im sorry
            {
                foreach (Function function in namespaceInfo.namespaceFuncitons)
                {
                    foreach (List<Command> functionCodeOverload in function.functionCode)
                    {
                        foreach (Command overloadCode in functionCodeOverload)
                        {
                            Global.currentLine = overloadCode.commandLine;
                            if (overloadCode.commandType == Command.CommandTypes.FunctionCall) overloadCode.functionCall.SearchCallFunction(namespaceInfo);
                            if (overloadCode.commandType == CommandTypes.CodeContainer) overloadCode.initCodeContainerFunctions(namespaceInfo);
                            if (overloadCode.commandType == CommandTypes.Calculation) overloadCode.calculation.InitFunctions(namespaceInfo);
                        }
                    }
                }

            }
            foreach (Command command in codeHeaderInformation.Item1)
            {
                Global.currentLine = command.commandLine;
                if (command.commandType == Command.CommandTypes.FunctionCall) command.functionCall.SearchCallFunction(codeHeaderInformation.Item2);
                if (command.commandType == CommandTypes.Calculation) command.calculation.InitFunctions(codeHeaderInformation.Item2);
                if (command.commandType == CommandTypes.CodeContainer) command.initCodeContainerFunctions(codeHeaderInformation.Item2);
            }

            return InterpretMain.InterpretNormalMode(codeHeaderInformation.Item1, initialAccessableObjects);

        }

    }
}
