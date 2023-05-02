namespace TASI
{
    internal class Tutorial
    {
        public static bool TutorialPhaseMinusOne()
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
            Console.Clear();
            Console.WriteLine("Perfect, let's start with the tutorial:");
            Console.WriteLine("We're gonna start by setting up the tutorial. Just create a basic text file and put \"type Tutorial0;\" (Without the quotation marks) in it. Why? I'll explain that later.");
            Console.WriteLine("After saving that file, start the interpreter, click away the test and drag and drop the file into the console window and press 'enter'. (Very specific)");
            Console.WriteLine("(Press any key to continue)");
            Console.ReadKey();
            Environment.Exit(0);
            return true;

        }
    }
}
