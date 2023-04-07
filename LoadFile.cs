namespace TASI
{
    internal class LoadFile
    {
        public static List<Command> ByPath(string location)
        {
            location = location.Trim('"');
            if (!File.Exists(location)) throw new Exception("The entered file doesn't exist.");
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
                if (line.Contains('Ⅼ')) throw new Exception($"Uhhhhmmm this is a weird error now. So basically, on line {i + 1} you used a character, that is already used by TASI to map code to lines (The character is:(I would have inserted it here right now, but the console can't even print this char. It looks like an L, but it's a bit larger.)). I picked this character, because I thought noone would use it directly in their code. Well, seems like I thought wrong... Simply said, you must remove this character from your code. But fear now! With the return statement \"lineChar\", you can paste this char into strings and stuff. I hope this character is worth the errors with lines! I'm sorry.\n-Ekischleki");
                allFileCode += $"Ⅼ{i}Ⅼ{line}";
            }
            Global.allLoadedFiles.Add(location);
            return StringProcess.ConvertLineToCommand(allFileCode);

        }
    }
}
