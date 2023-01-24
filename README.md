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
