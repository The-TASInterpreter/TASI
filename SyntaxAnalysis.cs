using System.Reflection.Metadata.Ecma335;
using TASI;

namespace TASI
{
    internal class SyntaxAnalysis
    {
        public static string AnalyseSyntax(List<Command> syntax)
        {
            string result = "Statement:";
            int initialResultLenght = result.Length;
            List<SyntaxAnalysisHelp> analysisObjects = new List<SyntaxAnalysisHelp>();
            //First line; Print out every command 
            foreach (Command command in syntax)
            {
                analysisObjects.Add(new(command));
                result += " " + command.originalCommandText;
            }
            result += "\n";
            for (int i = 0; i < initialResultLenght + 1; i++)
                result += " ";
            foreach (Command command in syntax)
            {
                for (int i = 0; i < command.originalCommandText.Length; i++)
                {
                    result += "=";
                }
                result += " ";
            }
            result += "\n";
            for (int i = 0; i < initialResultLenght + 1; i++)
                result += " ";
            for (int j = 0; j < syntax.Count; j++)
            {
                Command command = syntax[j];
                SyntaxAnalysisHelp analysisObject = analysisObjects[j];
                for (int i = 0; i < analysisObject.lenghtMiddle; i++)
                    result += " ";
                result += "|";
                for (int i = 0; i < analysisObject.lenghtMiddleRest; i++)
                    result += " ";

            }
            List<Line> lines = new List<Line> { new() };
            
            
                


            Console.WriteLine(result);
            return result;
        }

    }


    internal class Line
    {
        public int occupiedTill;
        public Line()
        {
            occupiedTill = 0;
        }
        public bool IsStillOccupied(int currentPos)
        {
            return occupiedTill < currentPos;
        }

    }

    internal class SyntaxAnalysisHelp
    {
        public Command syntaxCommand;
        public int commandLenght;
        public int lenghtMiddle;
        public int lenghtMiddleRest;
        public bool labeled = false;

        public SyntaxAnalysisHelp(Command syntaxCommand)
        {
            this.syntaxCommand = syntaxCommand;
            commandLenght = syntaxCommand.originalCommandText.Length;
            lenghtMiddle = commandLenght / 2;
            lenghtMiddleRest = commandLenght - lenghtMiddle;
        }
    }
}
