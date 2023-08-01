


namespace TASI
{

    public class Accessor
    {
        public enum AccessorType
        {
            offset, method, accessorBase
        }
        public AccessorType accessorType;
        public string? field;
        public MethodCall? methodCall;
        public Command? accessorBase;

        public Accessor(string field)
        {
            accessorType = AccessorType.offset;
            this.field = field;
        }
        public Accessor(MethodCall methodCall)
        {
            accessorType = AccessorType.method;
            this.methodCall = methodCall;
        }
        public Accessor(Command accessorBase)
        {
            accessorType = AccessorType.accessorBase;
            this.accessorBase = accessorBase;
        }
    }

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
        public List<Accessor> accessors;
        public void initCodeContainerFunctions(NamespaceInfo namespaceInfo, Global global)
        {
            foreach (Command command in codeContainerCommands)
            {
                if (command.commandType == CommandTypes.FunctionCall) command.functionCall.SearchCallNameObject(namespaceInfo, global);
                if (command.commandType == CommandTypes.CodeContainer) command.initCodeContainerFunctions(namespaceInfo, global);
                if (command.commandType == CommandTypes.Calculation) command.calculation.InitFunctions(namespaceInfo, global);

            }
        }



        public enum CommandTypes
        {
            FunctionCall, Statement, Calculation, String, CodeContainer, EndCommand, ObjectAccessor
        }
        /// <summary>
        /// For creating code containers
        /// </summary>
        /// <param name="codeContainerCommands"></param>
        /// <param name="commandLine"></param>
        /// <param name="commandEnd"></param>
        public Command(List<Command> codeContainerCommands, Global global, int commandLine = -1)
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

        public Command(List<Accessor> accessors, Global global, int commandLine = -1, int commandEnd = -1)
        {
            commandFile = global.CurrentFile;
            commandType = CommandTypes.ObjectAccessor;
            commandText = string.Empty;
            this.commandLine = commandLine;
            this.commandEnd = commandEnd;
            this.accessors = accessors;
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
                    this.codeContainerCommands = StringProcess.ConvertLineToCommand(commandText, global, commandLine);

                    originalCommandText = "{" + commandText + "}";
                    break;
                default:
                    originalCommandText = commandText;
                    break;
            }
        }
    }
}
