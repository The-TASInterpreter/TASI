# Text adventure script Interpreter
This is a project of mine, which is there, to make whitelisting storys unnessesary. Storys don't need to be whitelisted anymore, because dangerus functions will be disabled.
## Language basics
- There will be methods
- Everything will be defined in the Inf method
- Methods can have submethods
- At this point of development, I think, that there will be no costom datatypes
- There are 4 datatypes:
  - String for storing text
  - Num (aka double) for storing decimal numbers
  - Bool for storing true/false values
    - Bool values can also be used in calculations. A true bool value will be the int value of 1 and a false will be 0
  - int (aka long) for storing 64 bit whole numbers
- There are arrays for each datatype
- Every file is a different 'Class'
- The permissions of the programm will be defined by a supervisor class

## Syntax
1. At the beginning of every file, you must specify the class name. ``` DEFNAME <ProjectName>; ```
2. Every statement must end with ';'. Statements that have a 'Brace'({ or }) behind them, must not end with ';', but ig it would be nicer.
3. You define methods like this: ```method <method name> { Method contents }```
4. You declare methods using the inf method. 
   - You have to define a inf method
   - You have to use the ```INF.DefFunc <string name> <string return type> <string array input arguments 1. arg type 2. var name>``` method to define a func
   - This could look something like this: 
 ```method inf { [INF.DefFunc "Main", "void", DecArray string {"string"; "string_argument"; "num"; "num_argument";} ]; [INF.DefFuncVar.Num "Main","i",(0)]; }```


## At the current point of time
- You can call internal methods like this: ``` [<methodName>:<argument>,<argument>,...] ``` or ``` [<methodName>] ```
- If you just call a method, you dont need a semicolon after doing so.
- There are two types of statements: Return statements, which will return a value (variables also count as return statement) and static statements which dont return something, like while loops. TASI will determine which is expected and differentiate between them.
- After calling a static statement, you need a semicolon.
- Num calculation arent only for nums, but can also be used for strings.
- Num calculations are in brackets, here is an example: ``` (1 + 1 * (2 - 5))
- There are no real calculation rules in num calculations, its just from left to right and whats in brackets will be calculated together.

Here is some example code:


[Console.Clear] //Cleare anything thats written in the console
[Inf.DefVar:"Num","times"] //Create a var with the Num type called times
[Console.WriteLine:"Enter the amount of times you want to loop"] //Write to the console, so the user sees
set times [Convert.ToNum:[Console.ReadLine], true]; //set times to a, from string to num, converted userinput and fail if it cant be converted

[Inf.DefVar:"String","printText"] //Create a var with the String type called printText
[Console.WriteLine:"Enter the text you want to display"] //Write to the console, so the user sees
set printText [Console.ReadLine] //set printText to a userinput

[Inf.DefVar:"Num","i"] //Create a var with the Num type called i (it will be 0 as default)
while (!(($times) = ($i))) //Repeate while times does not equal to i
{
    [Console.WriteLine:(($printText) + " | " + (($i) + 1) + " out of " + ($times))]
    set i (($i) + 1);  //Set i to i + 1
}; 
[Programm.Pause:true]
