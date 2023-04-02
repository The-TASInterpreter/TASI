namespace Text_adventure_Script_Interpreter
{
    internal class LineString
    {
        public string @string;
        public List<int> stringLines = new();

        public LineString(List<string> inputStringList)
        {
            @string = "";
            for (int i = 0; i < inputStringList.Count; i++)
                for (int j = 0; j < inputStringList[i].Length; j++)
                {
                    @string += inputStringList[i][j];
                    stringLines.Add(i + 1);
                }


        }
    }
}
