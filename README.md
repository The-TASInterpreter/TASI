# Text adventure script Interpreter
This is a project of mine, which is there, to make whitelisting storys unnessesary. Storys don't need to be whitelisted anymore, because dangerus functions will be disabled.

## At the current point of time
TASI is not finished just yet but will hopefully soon. Some proper tutorials will come when the syntax are more or less 100% done.


Here is some example code:

```
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
[Programm.Pause:true] // Pause after programm is done.
```
This code is not executable at this stage of development but you can use this to do the same thing:
``` [Console.Clear][Inf.DefVar:"Num","times"][Console.WriteLine:"Enter the amount of times you want to loop"]set times [Convert.ToNum:[Console.ReadLine], true];[Inf.DefVar:"String","printText"][Console.WriteLine:"Enter the text you want to display"]set printText [Console.ReadLine][Inf.DefVar:"Num","i"]while (!(($times) = ($i))) {[Console.WriteLine:(($printText) + " | " + (($i) + 1) + " out of " + ($times))]set i (($i) + 1);};  ```
