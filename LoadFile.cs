using Codename_TALaT_CS;
using System.Text;
using static TASI.Command;
using SFM;

namespace TASI
{
    public class LoadFile
    {
        public static string GetGlobalEmulatedFile(string path)
        {
            InternalFileEmulation? result = Global.internalFiles.FirstOrDefault(x => x.path.ToLower() == path.ToLower(), null);
            return (result ?? throw new CodeSyntaxException("The path to the file doens't exist.")).Content;
        }

        public static List<Command> ByPath(string location, Global global, bool autoAddToGlobal = true)
        {
            StringBuilder sb = new();
            location = location.Trim('"');
            List<string> codeFile = LoadFile.GetGlobalEmulatedFile(location).Split('\n').ToList();
            for (int i = 0; i < codeFile.Count; i++)
            {
                sb.Clear();
                for (int j = 0; j < codeFile[i].Length; j++)
                {
                    if (codeFile[i][j] == '#') break; //Remove what comes next in the line, if there is a comment
                    if (codeFile[i][j] == 'Ⅼ') throw new CodeSyntaxException($"Uhhhhmmm this is a weird error now. So basically, on line {i + 1} you used a character, that is already used by TASI to map code to lines (The character is:(I would have inserted it here right now, but the console can't even print this char. It looks like an L, but it's a bit larger.)). I picked this character, because I thought noone would use it directly in their code. Well, seems like I thought wrong... Simply said, you must remove this character from your code. But fear now! With the return statement \"lineChar\", you can paste this char into strings and stuff. I hope this character is worth the errors with lines! I'm sorry.\n-Ekischleki");
                    sb.Append(codeFile[i][j]);
                }
                codeFile[i] = sb.ToString();
            }

            sb.Clear();
            for (int i = 0; i < codeFile.Count; i++)
            {
                sb.Append($"Ⅼ{i}Ⅼ{codeFile[i]}");
            }
            if (autoAddToGlobal)
                global.AllLoadedFiles.Add(location);
            return StringProcess.ConvertLineToCommand(sb.ToString(), global);

        }

        public static Value? RunCode(List<Command> tokenisedCode, Global global)
        {
            var codeHeaderInformation = InterpretMain.InterpretHeaders(tokenisedCode, "", global);
            AccessableObjects initialAccessableObjects = new(new(), codeHeaderInformation.Item2, global);

            foreach (NamespaceInfo namespaceInfo in global.Namespaces) //Activate functioncalls after scanning headers to not cause any errors. BTW im sorry
            {
                foreach (Function function in namespaceInfo.namespaceFuncitons)
                {
                    foreach (List<Command> functionCodeOverload in function.functionCode)
                    {
                        foreach (Command overloadCode in functionCodeOverload)
                        {
                            global.CurrentLine = overloadCode.commandLine;
                            if (overloadCode.commandType == Command.CommandTypes.FunctionCall) overloadCode.functionCall.SearchCallFunction(namespaceInfo, global);
                            if (overloadCode.commandType == CommandTypes.CodeContainer) overloadCode.initCodeContainerFunctions(namespaceInfo, global);
                            if (overloadCode.commandType == CommandTypes.Calculation) overloadCode.calculation.InitFunctions(namespaceInfo, global);
                        }
                    }
                }

            }
            foreach (Command command in codeHeaderInformation.Item1)
            {
                global.CurrentLine = command.commandLine;
                if (command.commandType == Command.CommandTypes.FunctionCall) command.functionCall.SearchCallFunction(codeHeaderInformation.Item2, global);
                if (command.commandType == CommandTypes.Calculation) command.calculation.InitFunctions(codeHeaderInformation.Item2, global);
                if (command.commandType == CommandTypes.CodeContainer) command.initCodeContainerFunctions(codeHeaderInformation.Item2, global);
            }

            return InterpretMain.InterpretNormalMode(codeHeaderInformation.Item1, initialAccessableObjects);

        }

        public static Value? RunCode(string code)
        {
            Global global = new();
            List<Command> tokenisedCode = StringProcess.ConvertLineToCommand(code, global);
            var codeHeaderInformation = InterpretMain.InterpretHeaders(tokenisedCode, "", global);
            AccessableObjects initialAccessableObjects = new(new(), codeHeaderInformation.Item2, global);

            foreach (NamespaceInfo namespaceInfo in global.Namespaces) //Activate functioncalls after scanning headers to not cause any errors. BTW im sorry
            {
                foreach (Function function in namespaceInfo.namespaceFuncitons)
                {
                    foreach (List<Command> functionCodeOverload in function.functionCode)
                    {
                        foreach (Command overloadCode in functionCodeOverload)
                        {
                            global.CurrentLine = overloadCode.commandLine;
                            if (overloadCode.commandType == Command.CommandTypes.FunctionCall) overloadCode.functionCall.SearchCallFunction(namespaceInfo, global);
                            if (overloadCode.commandType == CommandTypes.CodeContainer) overloadCode.initCodeContainerFunctions(namespaceInfo, global);
                            if (overloadCode.commandType == CommandTypes.Calculation) overloadCode.calculation.InitFunctions(namespaceInfo, global);
                        }
                    }
                }

            }
            foreach (Command command in codeHeaderInformation.Item1)
            {
                global.CurrentLine = command.commandLine;
                if (command.commandType == Command.CommandTypes.FunctionCall) command.functionCall.SearchCallFunction(codeHeaderInformation.Item2, global);
                if (command.commandType == CommandTypes.Calculation) command.calculation.InitFunctions(codeHeaderInformation.Item2, global);
                if (command.commandType == CommandTypes.CodeContainer) command.initCodeContainerFunctions(codeHeaderInformation.Item2, global);
            }

            return InterpretMain.InterpretNormalMode(codeHeaderInformation.Item1, initialAccessableObjects);

        }

    }
}
