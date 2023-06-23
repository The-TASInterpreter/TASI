

namespace TASI
{
    public class CommandLine
    {
        public List<Command> commands;
        public long lineIDX;



        public CommandLine(List<Command> commands, long lineIDX = -1)
        {
            this.commands = commands;
            this.lineIDX = lineIDX;
        }

        public void SaveToFile(string path)
        {
            List<string> file = new List<string>();
            foreach (Command command in commands)
            {
                file.Add(command.commandText);
                file.Add(command.commandType.ToString());
            }
            File.AppendAllLines(path, file);
        }

        public void LoadFromFile(string Path)
        {
            List<String> file = new List<string>(File.ReadAllLines(Path));
            for (int i = 0; i < file.Count; i += 2)
            {
                commands.Add(new Command(Enum.Parse<Command.CommandTypes>(file[i + 1]), file[i]));
            }
        }
    }
}
