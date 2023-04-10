# TASI - Documentation

TASI is an interpretet programming language, which is meant to be simple, intuitive and consistent. It is open source under the MIT License and its code can be found here under the following link "https://github.com/Ekischleki/TASI".

TASI has 6 token/command types, to which I will refer later in the documentation:
- Function calls
- Statements
- Calculations
- Strings
- Code containers
- End commands



## Code containers
This token type is more or less the base of this language. Code containers kinda describe themselfes, just a container for code. This code can then be interpreted in different interpretation modes, but the most common one is the "InterpretNormalMode". A code container is build up out of curly brackets, "{" marks the beginning of a code container, while "}" marks the end. Here is an example for a code container token:
```
{
    code;
}
```
Code containers can contain every type of token. E.g. can a code container contain another code container.

```
{									
    code;										
    {										
        code inside a code container inside a code container;		
    }										
}										
```






# Strings
A string is text, that is marked with \". By that logic, that means, that if you want to use \" inside a string, that it would end the string, but you can use it anyways with \\\". But what if you wanted to print out '\\\"'? You can print a '\\' by just putting another '\\' at the beginning. So you would print out '\\\"' like this: '\\\\\\\"'. Here are some example strings:

```
"So this is what they call a \"string\"..."
"Just some text, nothing else..."
"Inside here one can be free from all other types of token"
"I can put here anything I want, I just gotta be carefull with the \"'s, but you can put them in your strings anyways, by using '\\\"'"
"\"But doesn't it fail if you use a \"{\" or a \"}\"?\" they asked..."
"But I said: Didn't you listen?! These \"strings\" are free from any other type of token!"
```

## Comments
Comments aren't extra tokens and get treated extra. Comments are marked with a #, and it doesn't matter where it is, it will comment out everything till the end of the line. So if you put a # inside a string, it will comment out the whole end of the string. If you want to prevent that, you can put a \\ infront of the #. E.g.:
```
#A whole line can be a comment
"But also just a part of the line" #And this is it!
"So you can just place a \# anywhere to comment it out" #Just like every programming language ever lol.
"Comments are even more free from all types of token then I am" #Yes, this is because nothing can stop me!
"Except a newline..."
```


## Statements
There are two types of statement in TASI, the return statement and the normal statements. They don't differentiate in terms of syntax, they're both just written in plain and space seperated. They will also immediately end if they encounter sth. like a method, string ect.. Normal statements are used directly and don't return anything while return statements return a value, TASI knows weather a value is needed, and can therefore decide on which to use. Normal statement can therefore only be used directly, therefore they need an end command (semicolon aka ";") at the end
Here is a list of all statements in the normal interpret mode:

### Normal statements:

- return:
	When using a return statement inside a code container, the execution of the code in the code container will end and return a value. There are two types of usage for the return statement:
	1. ```return; Returns nothing and just exits the code container (returns a void value).```
	2. ```return <value>; Returns any value and exits the code container.```
	Example:
```
{
	return;
}
```
```
{
	return "...a string value";
}
```

- set:
	With this statement you can set a variable to a value. There is one type of the using for the set statement:
	1. ```set <statement: variable name> <value>;```
	Example:
```
set variable "string value";
```
```
set toastAmount "None, cause I ate it all";
```
- while:
	The while statement can repeat the code inside a code container (executed in normal mode), while a specific condition is true. If you use the return statement inside a while loop, it will return, what the return statement returned. Here is the usage for this statement:
	1. ```while <bool: condition> {<code to execute>};```
	Note that the while loop is a statement like any other, therefore it needs an end command at the end.
	Example:
```
while true 
{
	set stillRunning "Seems like it...";
};
```
- if:
	The if statement is meant to execute a code container (in normal mode), if some condition is true, if this condition is false, it can otherwhise execute another code container, but this is optional. If you use the return statement inside an if statement, it will return, what the return statement returned. The if statement has two different types of usage:
		1. ```if <bool: value> <code container>; # Will execute code container only, if value is true```
		2. ```if <bool: value> <code container1> else <code container2>; # Will execute code container 1 if value is true, otherwise it will execute code container 2```

These were all meaningfull normal statements (till now) there are still some more statements for helping with methods and namespaces: helpm, listm and rootm. But instead of explaining them, I encourage you to try them out yourself and let TASI tell you more about them. Just don't forget the end command!
Most statements will, when they need a value, only accept single-line return-statements. You can use the following to get aroung that limitation: ```($ <multi line return statement>)```

### Return statements:

- true:
	does nothing and returns a bool-type with the value of true.
- false:
	does nothing and returns a bool-type with the value of false (ctrl + c, ctrl + v. Yes yes, very boring).
- void:
	does nothing and returns a void-type (idk why you would wanna use that, but there must be a case. That's why I put it in.
- nl:
	does nothing and returns a string-type with the value of \n (new line). TASI doesn't have something like \n, so you must use this return statement instead.
- if:
	Yes, if again?! Well let me explain. This is an if statement, with only the if else variant. You can use the return statement inside of the code containers, the tell the if return-statement what to return.
		Example:
```
set variable ($ if false {return "conspiracy?! This should never be the value of \"variable\"";} else {return "Much better! After this, variable should be equal to this text";}); # I will explain these weird braces soon, they're just here, because most of the statements only support return-statements that are one statement long. I could code it to make it work without these braces, but why if there's already a way to make it work?
```
Here is the example, but it's split up, so you can read it better:
```
set variable ($ 
	if false 
	{
		return "conspiracy?! This should never be the value of \"variable\"";
	} 
	else 
	{
		return "Much better! After this, variable should be equal to this text";
	}
);
```
- do:
	Just like the return-statement if, just that you don't need a condition and only have one code container that can will be executed. You can use the return statement too, to make it return a value.
		Example:
``` 
set variable ($ do { set variable "value"; return "Different and final value";}); 
```

**VARIABLES ARE RETURN-STATEMENTS TOO!!! It's very important to keep that in mind!**



## Function calls
Let's talk about one of the most important parts of this language. Function calls are somewhat like a black box - you put something in (sometimes), something comes out (sometimes). So methods can have an input (they must not have an input) and they can also return something (they can also return a void). We'll talk about defining your own functions later (in the header part), but now well just talk about function calls. A function call is build up like this: 
```
[<function name>:<function arguments; comma seperated>]
```
or
```
[<function name>]
```

You can use function calls directly in normal interpreting mode, but you can also use them as values, because they return values.
The function name is build up like this:
```
<namespace>.<function name>
```
What a namespace is, will we discuss later (in the header part)
Here are some examples of directly calling a function:
```
[Console.ReadLine]
[Console.WriteLine:"This is a function call with one argument, therefore we don't need a comma to seperate arguments, because there's only one. The method before that had zero arguments, so it didn't even need a colon to sepperate args from name"]
[Test.HelloWorld:true, "This function call now has two arguments and we can keep going like this"]
```
Here are some examples of using functions as values:
```
set stringVar [Function.ThatReturnsThisString:"this string?!"]; # This will set the variable stringVar to the output of "Function.ThatReturnsThisString"
[Console.WriteLine:[Console.ReadLine]] #This will print the output of "Console.ReadLine" out to the screen. BTW. when you use a function call directly, you don't need a semicolon after it.
```


## Calculations
Calculations are used to combine multible values or things into one value. Calculations are always in braces. There are operators, which I'll list in a sec. The normal calculation rules like the ones in math don't apply. It's just left to right and if theres something in braces, calcualte it in a batch. If you want to provide a number value, you need to put it in a calculation like this:

```
set numVar (14);
```

something like
```
set numVar 14; #THIS WONT WORK
```
would not work, because 14 wouldn't be percieved as a number but rather as a statement by TASI. To make it be percieved as a number, you need the calculation. At the end of the caluculation there can only be one value left. Values and operators must be space seperated. If you want to use return statements (like variables. Did you keep it in mind? I told you it would be important) inside a calculation, you need a return statement calculation brace. It's basically like a normal calculation brace, just that you have to put a '$' at the first position inside the brace.

```
set numVar ($variable); #BECAUSE VARIABLES ARE RETURN STATEMENTS TOO
set numVar (15 + ($variable));

set boolVarUserEnteredJEEP ($ if ([Console.ReadLine] == "JEEP") {return true;} else {return false;});
```

Functions, strings and other calculations can be used as normal inside calculations:
```
set stringVar ("Hello W" + (1 - 1) + [Function.ThatReturnsRLD]); #This would be Hello W0rld
```
If you use a bool value inside a calculation, it will be converted to a num value (true becomes 1 and false becomes 0).

Before we come to the operators, I'll tell you a little fun fact, did you know, that because of the handling of calculation, something like this would be possible:
```
set youLLGoToHellIfYouUseThis (15 5 + - + 25); #This is 5 lmao
```
but as I said, you'll probably go to hell if you use this kind of syntax.

Operators:

- "+" 			that's the addition operator. It accepts 2 numeric- and string-type values. If you add string-types, this operator will join them together. If you add a string-type with a numeric-type, it will treat both as strings. E.g.: ("1" +  1) is 11
- "-" 			that's the subtraction operator. It accepts 1 or 2 numeric-type values. If you use it with 1 value, it will invert the sign of the value. If you use 2 values, it's just a normal subtraction.
- "*" 			that's the multiplication operator. It accepts 2 numeric-type values and is just a normal multiplication.
- "/" 			that's the devision operator. It accepts 2 numeric-type values and is just a normal devision.
- "and"			that's the and operator. It accepts 2 bool (or automaticly to bool converted) values and will return true, if both are true.
- "or" 			that's the or operator. It accepts 2 bool (or automaticly to bool converted) values and will return true, if at least one of both values is true.
- "!" or "not"	that's the not operator. It accepts 1 bool (or automaticly to bool converted) value and will invert the bool.
- "%"			that's the mod operator. It accepts 2 numeric-type values and is just a normal mod operation.
- "="			that's the non strict equal operator. It needs at least 3 values. It will convert all values to the in the first value specified type and then compare them. If one value can't be converted, it will return false. So ("bool" "Hi" = "Hi") is false, because "Hi" can't be converted to a bool. If you have more than 3 values, it will compare all of them. E.g. ("string" 23 "23" = (20 + 3) "23") will return true, because if you convert all values to a string, they all have the same value. If you're not sure how to use multible values per operator, don't use them.
- "=="			that's the type strict equal operator. It needs at least 2 values. If all values don't have the same type, it will return false. E.g. (15 == "15") is false. It has the same acceptance rules as the non strict equal operator and can also compare multible values.
- "<"			that's the less than operator. It accepts 2 numeric-type values and is just a less than operation.
- ">"			that's the greater than operator. It accepts 2 numeric-type values and is just a greater than operation.

Maybe there will be a way to define your own operators one day. But this day is not today.


Header
Every file is its own namespace. You need to define the properties of the namespace at the Header layer so at the most outer layer. This layer will be interpreted in header mode, where you can't use method calls and can only use header statements:

- name:
	The name statement is built up like this: name <statement: namespace name>; and can be used, to define the namespace name of the current file.
- type:
	The type statement is built up like this: type <statement: type>; and can be used, to set the type of the current namespace. Types are:	 
		- supervisor:
				Not implemented yet, but will be able to start other files and only allowing specefic stuff.
		- generic:
			A generic namespace with a start part.
		- library:
			A library namespace, can't get started directly, and can only be used, if you import it from another namespace.
		- internal:
			An internal namespace. You can't create those, but the still exist. It's used for internal functions, like "Console.WriteLine"
	
- start:
	The start statement is built up like this: start {<start code>}; and it contains the start code of the namespace, that'll get executed, if it gets started.
- function:
		The function statement is built up like this: ```function <statement: return-type> <statement: function name> {<input types in var def interpreter mode>} {<method code>};``` the function statement is used, to define your own functions. You're probably still confused because of the var def interpreter mode. But don't worry, I'll talk about it in a second.

- import:
		The import statement is built up like this: ```import <string: full path>;``` or ```import base <string: path from current file on>;```  and it can import functions of other namespaces to the current. 
An example for a header:

```
name HeaderExampleLibrary;
type library;
function num ReturnRandomValue {num seed;} 
{
    set seed [Convert.ToNum:(($seed) + "3"), true];
    return (($seed) * ($seed) * ($seed) * ($seed) % 10000);
};
```
```
name HeaderExampleNamespace;
type Generic;
import base "HeaderExampleLibrary.TASI"; #You would need the full path of the file with just the import <string path>; statement

start 
{
[Inf.DefVar:"num","seed"]
set seed (12);
while true 
{
set seed [HeaderExampleLibrary.ReturnRandomValue:seed];
[Console.WriteLine:seed]
};
};
```

## var def interpreter mode
This is just another interpreter mode just like header mode, just with some different rules. Its only purpose is to define a list of variables and it only has one statement type. The statement is built up like this:
```
<statement var-type> <var name>;
```
Here are some examples:
```
num numberVariable; 
string stringVariable; 
num aDifferentNumberVariable;
bool aBool;
```

Btw.: all available types in TASI are:

- num - an internal double number with general purpose number use.
- string - a text with general purpose text use.
- bool - a true or false value which is internaly just another double that's either 1 or 0
- void - void variables don't store values and just symbolise nothing.

Other stuff you may want to know:
- Line numbers are not so reliable - When you get an error in your code, it shows you a line number where the error happened (most of the time). This number is not that reliable at the current point of time. It might say the error's on line 3, but in reality it's on line 14. So don't search for an error that doesn't exist.

- Internal errors should get reported - Sometimes there's nothing wrong with your TASI code, but with my code. If you get an error that starts with an "Internal:", please make a new issue and send me the error and your code.

- TASI is not case sensitive - Most of the time TASI is just not case sensitive. I tried to stay consistent with that most of the time, but it can happen, that it just becomes case sensitive again. If you find a case like that, report it to me.

- There is stuff that isn't in the documentation - ...and it has a reason. Like E.g. that you can return a return like this return return; you don't need to know why and isn't it in the documentation now?! OH NO!

- In general report errors - yeah just write an issue.

- I can't write documentations - If you want to write a better documentation, PLEASE do that. Otherwise if you have any questions about the language, please just ask (just ask me and not on stack overflow; they'll vote you down - 6 feet under...)

- You should maybe read this again to fully understand everything...

Best of luck with TASI! You can get the newest release here: https://github.com/Ekischleki/TASI/releases
Keep in mind, that what's been written here might not 100% reflect what's actually true. TASI is a language, that's still in development. I'll try to update the documentation with the latest changes.






























