

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


    }
}
