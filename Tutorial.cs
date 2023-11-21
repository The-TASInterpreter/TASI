namespace TASI
{
    internal class Tutorial
    {
        public static bool TutorialPhaseMinusOne(bool wasError = true)
        {
            if (wasError)
            {
                Console.Clear();
                Console.WriteLine("You had some quite weird error, that's why I'll just assume, that you don't know what to do in this language.");
                Console.WriteLine("If I just insulted you by saing that you don't know what you're doing, I'm sorry.");
                Console.WriteLine("If you actually do know what you're doing, and don't want a tutorial in the language just press 'N'.");
                Console.WriteLine("Otherwhise, if you do want a tutorial, press 'Y'.");
                char userInput;
                do
                {
                    userInput = Console.ReadKey().KeyChar.ToString().ToUpper()[0];
                } while (userInput != 'Y' && userInput != 'N');
                if (userInput == 'N')
                    return false;
            }
            Console.Clear();
            Console.WriteLine("Perfect, let's start with the tutorial:");
            Console.WriteLine("We're gonna start setting up the tutorial. Just create a basic text file and put \"type Tutorial0;\" (Without the quotation marks) in it. Why? I'll explain that later.");
            Console.WriteLine("After saving that file, start the interpreter, drag and drop the file into the console window, and press 'enter'. (Very specific)");
            AnyKeyToContinue();
            return true;

        }
        public static void TutorialPhase0()
        {
            Console.Clear();
            Console.WriteLine("You made it to phase 0!");
            Console.WriteLine("So let's start by talking about code containers.");
            Console.WriteLine("This token type is more or less the base of this language. Code containers kinda describe themselfes: just a container for code. This code can then be interpreted in different interpretation modes, but the most common one is the \"InterpretNormalMode\". A code container is built up out of curly brackets. \"{\" marks the beginning of a code container, while \"}\" marks the end.");
            Console.WriteLine("Here's an example of a code container:\n{\n\t<some code here>\n}");
            Console.WriteLine("Code containers can contain every type of token. E.g. a code container can contain another code container. Like this:");
            Console.WriteLine("{\n\t{\n\t\t<code inside a code container inside a code container>\n\t}\n}");
            Console.WriteLine("\nNow, to progress to the next phase, update the \"type Tutorial0;\" to \"type Tutorial1;\", and create a code container inside a code container underneeth the \"type Tutorial1;\". Nothing else! Then run it in the interpreter again.");
            AnyKeyToContinue();

        }

        public static void TutorialPhase1(List<Command> codeFile)
        {
            Console.WriteLine();
            if (codeFile.Count != 4)
            {
                Console.WriteLine("That wasn't quite right. As I said, code container in code container underneeth \"type Tutorial1;\". NOTHING ELSE. This happened, because you had something else. No creativity allowed!");
                AnyKeyToContinue();
            }
            if (codeFile[3].commandType != Command.CommandTypes.CodeContainer)
            {
                Console.WriteLine("That wasn't quite right. As I said, code container in code container underneeth \"type Tutorial1;\". This happened, because that wasn't a code container. If you want to see what a code container is again, update \"type Tutorial1;\" to \"type Tutorial0;\"");
                AnyKeyToContinue();
            }
            if (codeFile[3].codeContainerCommands.ToList().Count != 1)
            {
                Console.WriteLine("You got there half way. As I said, code container in code container underneeth \"type Tutorial1;\". NOTHING ELSE. This happened, because you had something else. No creativity allowed!");
                AnyKeyToContinue();
            }
            if (codeFile[4].codeContainerCommands.ToList()[0].commandType != Command.CommandTypes.CodeContainer)
            {
                Console.WriteLine("You got there half way. As I said, code container in code container underneeth \"type Tutorial1;\". This happened, because there wasn't another code container inside the initial code container, but another token type.");
                AnyKeyToContinue();
            }
            Console.WriteLine("You did it! First try for sure.");
            Console.WriteLine("The next thing is strings!\nStrings are a way to define and write text in your code. You start a string with \" and also end it with \". Here's an example:\n\"This is a string. It's just a basic way of writing text.\"");
            Console.WriteLine("Strings can contain all kinds of letters like \"{\" and \"}\". But what id you wanted to put a \" inside a string without ending it?\nThis is what char escaping is used for. If you want to escape a char (like \") you need to put a \\ infront of it. HereÄs an example on how to do that:\n\"So this is what they call a \\\"string\\\"...\"");
            Console.WriteLine("If you want to use \\ just do it like this: \"\\\\\"");
            Console.WriteLine("There are also other escape characters like a newline with \\n or a tab with \\t.");
            Console.WriteLine("Now it's your turn. Modify your \"type Tutorial0;\" to a \"type Tutorial1;\", remove the code containers and put some strings under the type. You need to include at least one string with a \" in it.");
            AnyKeyToContinue();
        }



        public static void AnyKeyToContinue()
        {
            Console.WriteLine("(Press any key to continue)");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
