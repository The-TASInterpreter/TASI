

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
        private int? commandEnd;

        public int CommandEnd
        {
            get
            {
                if (commandEnd == null)
                {
                    if (commandType == CommandTypes.CodeContainer)
                    {
                        if (codeContainerCommands.Any())
                        {
                            commandEnd = codeContainerCommands.Last().CommandEnd;
                        }
                        else
                        {
                            commandEnd = commandLine;
                        }

                    }
                    else throw new InternalInterpreterException("End was not defined");
                }
                return commandEnd ?? 0;
            }
            set
            {
                commandEnd = value;
            }
        }

        public string originalCommandText;
        public IEnumerable<Command>? codeContainerCommands;
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
        /// <param name="codeContainerCommands"></param>
        /// <param name="commandLine"></param>
        /// <param name="commandEnd"></param>
        public Command(IEnumerable<Command> codeContainerCommands, Global? global, int commandLine = - 1)
        {
            if (global != null)
                commandFile = global.CurrentFile;
            commandType = CommandTypes.CodeContainer;
            commandText = string.Empty;
            this.commandLine = commandLine;

            
            this.codeContainerCommands = codeContainerCommands;
            
        }

        /// <summary>
        /// General purpose command creation
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandLine"></param>
        /// <param name="commandEnd"></param>
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
