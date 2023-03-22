namespace TASI
{
    internal class ScanFile
    {
        public static List<CommandLine> ScanFilePathToCommand(string path)
        {
            List<string> file = new List<string>(File.ReadAllLines(path));
            List<CommandLine> result = new List<CommandLine>();
            TASI_Main.line = 0;
            long lineIDX = 0;
            foreach (string line in file)
            {
                TASI_Main.line++;
                lineIDX++;
                if (line == String.Empty)
                    continue;
                result.Add(new CommandLine(StringProcess.ConvertLineToCommand(line), lineIDX));
                if (result[result.Count - 1].commands.Count == 0) //If latest line of file has no commands: remove it
                    result.RemoveAt(result.Count - 1);
                
            }
            return result;
        }
    }
}
