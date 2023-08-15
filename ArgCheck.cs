
namespace TASI
{
    internal class ArgCheck
    {
        public static readonly List<ArgDefinition> argCommandsDefinitions = new()
        {
            new("p", "enables you to import plugins. usage: /p <plugin path>", new() {1}, true ),
            new("r", "specifys the file that will be interpreted. usage: /r <file path>", new() {1 }, false),
            new("a", "loads a namespace specifyed and auto-imports it to every file used. usage /a <namespace path>", new() {1}, true)


        };

        public static void InterpretArguments(List<ArgInstance> tokens, Global global)
        {
            foreach (ArgInstance token in tokens)
            {
                switch (token.argDefinition.argName)
                {
                    case "p":
                        if (!File.Exists(token.argAttributes[0]))
                            throw new ArgumentException($"The plugin file \" {token.argAttributes[0]} doesn't exist");

                        global.Plugins.AddRange(PluginManager.PluginManager.GetPluginsFromAssembly(PluginManager.PluginManager.LoadPluginAssembly(token.argAttributes[0])));
                        break;
                    case "r":
                        throw new NotImplementedException();
                    case "a":
                        throw new NotImplementedException();
                    default:
                        throw new InternalInterpreterException($"Not implemented argument {token.argDefinition.argName}");
                }
                token.AlreadyUsed = true;
            }
        }



        public static List<ArgInstance> TokeniseArgs(string[] args, List<ArgDefinition> argDefinitions)
        {
            ArgInstance? currentArg = null;
            List<ArgInstance> result = new();

            for (int i = 0; i < args.Length; i++)
            {
                string? arg = args[i];
                if (arg[0] == '/' || arg[0] == '-')
                {
                    if (currentArg != null)
                    {
                        result.Add(currentArg);
                    }

                    if (arg.Length < 2)
                    {
                        throw new ArgumentException("There are no empty arguments (\"/\")");
                    }
                    string actualArg = arg[1..];
                    currentArg = new(argDefinitions.FirstOrDefault(x => x.argName == actualArg) ?? throw new ArgumentException($"An argument with the name \"{actualArg}\" doesn't exist. Use the /help argument to see all arguments."));


                }
                else
                {
                    if (currentArg == null)
                        throw new ArgumentException("You must first define an argument before listing attributes.");
                    currentArg.argAttributes.Add(arg);
                }

            }
            if (currentArg != null)
            {
                result.Add(currentArg);
            }
            result.ForEach(a => a.VerifyAttributes());

            return result;
        }

    }
    internal class ArgInstance
    {
        public ArgDefinition argDefinition;
        public List<string> argAttributes;
        private bool alreadyUsed;
        public bool AlreadyUsed
        {
            get
            {
                return alreadyUsed;
            }
            set
            {
                if (alreadyUsed == true)
                    throw new ArgumentException($"The argument {argDefinition.argName} can only be used once");
                alreadyUsed = value;
                if (argDefinition.usableMultibleTimes)
                    alreadyUsed = false;

            }
        }
        public ArgInstance(ArgDefinition argDefinition)
        {
            this.argDefinition = argDefinition;
            argAttributes = new();

        }

        public void VerifyAttributes()
        {
            if (!argDefinition.argAttributes.Contains(argAttributes.Count))
            {
                throw new ArgumentException($"Invalid usage of {argDefinition.argName} argument. Type help to see correct uses");
            }
        }
    }


    internal class ArgDefinition
    {
        public string argName;
        public string argDescription;
        public List<int> argAttributes;
        public readonly bool usableMultibleTimes;


        public ArgDefinition(string argName, string argDescription, List<int> argAttributes, bool usableMultibleTimes)
        {
            this.usableMultibleTimes = usableMultibleTimes;
            this.argName = argName;
            this.argDescription = argDescription;
            this.argAttributes = argAttributes;
        }
    }
}
