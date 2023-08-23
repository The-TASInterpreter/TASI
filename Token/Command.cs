

using TASI.InternalLangCoreHandle;
using TASI.RuntimeObjects.FunctionClasses;
using TASI.Token;

namespace TASI
{
    public class Command
    {
        public string commandText;
        public CommandTypes commandType;
        public int commandLine;
        public int commandEnd;
        public string originalCommandText;
        public List<Command>? codeContainerCommands;
        public FunctionCall? functionCall;
        public CalculationType? calculation;
        public string commandFile = "";
        public void initCodeContainerFunctions(NamespaceInfo namespaceInfo, Global global)
        {
            foreach(Command command in codeContainerCommands)
            {
                if (command.commandType == CommandTypes.FunctionCall) command.functionCall.SearchCallFunction(namespaceInfo, global);
                if (command.commandType == CommandTypes.CodeContainer) command.initCodeContainerFunctions(namespaceInfo, global);
                if (command.commandType == CommandTypes.Calculation) command.calculation.InitFunctions(namespaceInfo, global);

            }
        }


        
        public enum CommandTypes
        {
            FunctionCall, Statement, Calculation, String, CodeContainer, EndCommand
        }
        /// <summary>
        /// For creating code containers
        /// </summary>
        /// <param Name="codeContainerCommands"></param>
        /// <param Name="commandLine"></param>
        /// <param Name="commandEnd"></param>
        public Command(List<Command> codeContainerCommands, Global global, int commandLine = - 1)
        {
            commandFile = global.CurrentFile;
            commandType = CommandTypes.CodeContainer;
            commandText = string.Empty;
            this.commandLine = commandLine;
            if (codeContainerCommands.Any())
            {
                commandEnd = codeContainerCommands.Last().commandEnd;
            }
            else
            {
                commandEnd = commandLine;
            }

            
            this.codeContainerCommands = codeContainerCommands;
            
        }

        /// <summary>
        /// General purpose command creation
        /// </summary>
        /// <param Name="commandType"></param>
        /// <param Name="commandText"></param>
        /// <param Name="commandLine"></param>
        /// <param Name="commandEnd"></param>
        public Command(CommandTypes commandType, string commandText, Global? global, int commandLine = -1, int commandEnd = -1)
        {
            this.commandText = commandText;
            this.commandType = commandType;
            this.commandLine = commandLine;
            this.commandEnd = commandEnd;
            if (global != null)
                commandFile = global.CurrentFile;
            switch (commandType)
            {
                case CommandTypes.FunctionCall:
                    this.functionCall = new(this, global);
                    originalCommandText = $"[{commandText}]";
                    break;
                case CommandTypes.Calculation:
                    originalCommandText = $"({commandText})";

                    this.calculation = new(this, global ?? throw new InternalInterpreterException("Global was null"));
                    break;
                case CommandTypes.String:
                    originalCommandText = $"\"{commandText}\"";
                    break;
                case CommandTypes.CodeContainer:
                    this.codeContainerCommands = Tokeniser.CallTokeniseInput(commandText, global, commandLine);

                    originalCommandText = "{" + commandText + "}";
                    break;
                default:
                    originalCommandText = commandText;
                    break;
            }
        }
    }
}
  