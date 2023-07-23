
using NUnit.Framework;
using System;
using System.Diagnostics;
using TASI.Token;

namespace TASI
{


    [TestFixture]
    public class Benchmarks
    {
        [Test]
        public static void BenchmarkAnalysisString()
        {
            Stopwatch benchmark = new();
            benchmark.Start();
            Global global = new Global();
            for (int i = 0; i < 1000000; i++)
            {
                StringProcess.ConvertLineToCommand("\"1\"\"4\"\"789\"", global);
            }
            benchmark.Stop();
            Console.WriteLine($"Benchmark took {benchmark.ElapsedMilliseconds}ms");

        }
        [Test]
        public static void BenchmarkAnalysisStatement()
        {
            Global global = new Global();
            Stopwatch benchmark = new();
            benchmark.Start();
            for (int i = 0; i < 1000000; i++)
            {
                StringProcess.ConvertLineToCommand("one four nt", global);
            }
            benchmark.Stop();
            Console.WriteLine($"Benchmark took {benchmark.ElapsedMilliseconds}ms");
        }
    }


    [TestFixture]
    public class TokenTests
    {
        static Global global = new Global();
        [Test]
        public static void SringTest()
        {
            global = new Global();
            Command testCommand = StringProcess.HandleString("some statement \"\\nSome \\\"string\\\"\" some after that", 15, out int end, out _, global);
            Assert.AreEqual(  "\nSome \"string\"", testCommand.commandText);
            Assert.AreEqual(33, end);
        }
        [Test]
        public static void SringPromise()
        {
            global = new Global();
            CancellationTokenSource ct = new();
            PromisedString promisedString = new(ct, () =>
            {
                string result = "";
                while (true)
                {
                    ct.Token.ThrowIfCancellationRequested();
                    result += "Test";
                }
            });
            promisedString.Result = "Something";
            Assert.AreEqual("Something", promisedString.Result);
            ct = new();

            promisedString = new(ct, () =>
            {
                Thread.Sleep(100);
                return "Hello World!";
            });
            Assert.AreEqual("Hello World!", promisedString.Result);

        }
    }

    [TestFixture]
    public class CalcTests
    {
        [Test]
        public static void CalculationNumTest()
        {
            AccessableObjects accessableObjects = new(new(), new(NamespaceInfo.NamespaceIntend.nonedef, ""), new());


            Assert.AreEqual(4.5, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "((4 + 6) * 2 - (3 - 2)) / (1 + 1) % 5", accessableObjects.global), accessableObjects).NumValue);

            Assert.AreEqual(5, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "2 + 3", accessableObjects.global), accessableObjects).NumValue);

            Assert.AreEqual(2, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "5 - 3", accessableObjects.global), accessableObjects).NumValue);

            Assert.AreEqual(6, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "2 * 3", accessableObjects.global), accessableObjects).NumValue);

            Assert.AreEqual(20, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "2 + 3 * 4", accessableObjects.global), accessableObjects).NumValue);

            Assert.AreEqual(12, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "3 + (3 * 3)", accessableObjects.global), accessableObjects).NumValue);

            Assert.AreEqual(2.8, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "(2 * (3 + 4)) / 5", accessableObjects.global), accessableObjects).NumValue);

            Assert.AreEqual(6.0, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "3.14 + 2.86", accessableObjects.global), accessableObjects).NumValue);

            Assert.AreEqual(-2, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "-5 + 3", accessableObjects.global), accessableObjects).NumValue);
        }

        [Test]
        public static void CalculationStringTest()
        {
            AccessableObjects accessableObjects = new(new(), new(NamespaceInfo.NamespaceIntend.nonedef, ""), new());


            Assert.AreEqual("2apples", Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "2 + \"apples\"", accessableObjects.global), accessableObjects).StringValue);

            Assert.AreEqual("pears3", Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "\"pears\" + 3", accessableObjects.global), accessableObjects).StringValue);

            Assert.AreEqual("redApple", Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "\"red\" + \"Apple\"", accessableObjects.global), accessableObjects).StringValue);

            Assert.AreEqual("20Apple", Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "(2 + 3) * 4 + \"Apple\"", accessableObjects.global), accessableObjects).StringValue);

            Assert.AreEqual("-5 Cats", Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "-5 + \" Cats\"", accessableObjects.global), accessableObjects).StringValue);

            Assert.AreEqual("15", Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "\"\" + 1 + 5", accessableObjects.global), accessableObjects).StringValue);

        }
        [Test]
        public static void CalculationBoolTest()
        {
            AccessableObjects accessableObjects = new(new(), new(NamespaceInfo.NamespaceIntend.nonedef, ""), new());


            Assert.AreEqual(true, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "1", accessableObjects.global), accessableObjects).BoolValue);

            Assert.AreEqual(false, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "0", accessableObjects.global), accessableObjects).BoolValue);

            Assert.AreEqual(true, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "\"true\"", accessableObjects.global), accessableObjects).BoolValue);

            Assert.AreEqual(false, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "\"false\"", accessableObjects.global), accessableObjects).BoolValue);

            Assert.AreEqual(true, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "2 + 4 == 6", accessableObjects.global), accessableObjects).BoolValue);

            Assert.AreEqual(false, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "2 + 4 == \"6\"", accessableObjects.global), accessableObjects).BoolValue);

            Assert.AreEqual(true, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "2 + 4 + \"\" == \"6\"", accessableObjects.global), accessableObjects).BoolValue);

            Assert.AreEqual(true, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "\"string\" (2 + 4) = \"6\"", accessableObjects.global), accessableObjects).BoolValue);

            Assert.AreEqual(true, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "\"num\" (2 + 4) = \"6\"", accessableObjects.global), accessableObjects).BoolValue);

            Assert.AreEqual(false, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "\"bool\" (2 + 4) = \"6\"", accessableObjects.global), accessableObjects).BoolValue);

            Assert.AreEqual(true, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "5 > 2", accessableObjects.global), accessableObjects).BoolValue);

            Assert.AreEqual(false, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "5 < 2", accessableObjects.global), accessableObjects).BoolValue);

            Assert.AreEqual(true, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "-5 < 2", accessableObjects.global), accessableObjects).BoolValue);

            Assert.AreEqual(true, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "! false", accessableObjects.global), accessableObjects).BoolValue);

            Assert.AreEqual(false, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "!(1 and 1)", accessableObjects.global), accessableObjects).BoolValue);

            Assert.AreEqual(true, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "!(1 and false)", accessableObjects.global), accessableObjects).BoolValue);

            Assert.AreEqual(true, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "1 or 0", accessableObjects.global), accessableObjects).BoolValue);

        }
        [Test]
        public static void CalculationReturnStatementTests()
        {
            AccessableObjects accessableObjects = new(new(), new(NamespaceInfo.NamespaceIntend.nonedef, ""), new());
            accessableObjects.accessableVars.Add("helloworld", new Var(new VarConstruct(VarConstruct.VarType.@string, "helloWorld"), new(Value.ValueType.@string, "Hello World!")));
            accessableObjects.accessableVars.Add("num-pi", new Var(new VarConstruct(VarConstruct.VarType.num, "num-pi"), new(Value.ValueType.num, -3.141)));

            Assert.AreEqual(true, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "helloWorld == \"Hello World!\"", accessableObjects.global), accessableObjects).BoolValue);

            Assert.AreEqual("Hello World!5", Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "helloWorld + 5", accessableObjects.global), accessableObjects).StringValue);

            Assert.AreEqual(true, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "($ if (num-pi == -3.141) {return true;} else {return false;} )", accessableObjects.global), accessableObjects).BoolValue);

            Assert.AreEqual(true, Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "($ do {return true;})", accessableObjects.global), accessableObjects).BoolValue);

        }

        [Test]
        public static void CalculationExceptionTests()
        {
            AccessableObjects accessableObjects = new(new(), new(NamespaceInfo.NamespaceIntend.nonedef, ""), new());


            Assert.Throws<CodeSyntaxException>(() =>
            {
                Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "1 2", accessableObjects.global), accessableObjects);
            });

            Assert.Throws<CodeSyntaxException>(() =>
            {
                Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "1 2 + 4", accessableObjects.global), accessableObjects);
            });

            Assert.Throws<CodeSyntaxException>(() =>
            {
                Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "5 / 0", accessableObjects.global), accessableObjects);
            });

            Assert.Throws<CodeSyntaxException>(() =>
            {
                Calculation.DoCalculation(new(Command.CommandTypes.Calculation, "23 - \"Ahhhhh\"", accessableObjects.global), accessableObjects);
            });



        }
    }

    [TestFixture]
    public class InternalFunctionTests
    {
        [Test]
        public static void FunctionConsoleReadLine()
        {
            Global global = new();
            AccessableObjects accessableObjects = new(new(), new(NamespaceInfo.NamespaceIntend.nonedef, "", global), global);

            StringReader sr = new StringReader("Test!");
            Console.SetIn(sr);
            FunctionCall functionCall = new(new Command(Command.CommandTypes.FunctionCall, "Console.ReadLine", accessableObjects.global), accessableObjects.global);
            functionCall.SearchCallFunction(new(NamespaceInfo.NamespaceIntend.nonedef, "", accessableObjects.global), accessableObjects.global);
            Assert.AreEqual("Test!", functionCall.DoFunctionCall(accessableObjects).StringValue);

        }
        [Test]
        public static void FunctionConsoleWriteLine()
        {
            Global global = new();
            AccessableObjects accessableObjects = new(new(), new(NamespaceInfo.NamespaceIntend.nonedef, "", global), global);

            StringWriter sw = new();
            Console.SetOut(sw);
            FunctionCall functionCall = new(new Command(Command.CommandTypes.FunctionCall, "Console.WriteLine:\"Test\"", accessableObjects.global), accessableObjects.global);
            functionCall.SearchCallFunction(new(NamespaceInfo.NamespaceIntend.nonedef, "", accessableObjects.global), accessableObjects.global);
            functionCall.DoFunctionCall(accessableObjects);
            string consoleOutput = sw.ToString();
            Assert.AreEqual("Test\n", consoleOutput.Replace("\r\n", "\n"));

        }

    }
    [TestFixture]
    public class ThreadingTests
    {
        [Test]
        public static void PromiseTest()
        {
            Global global = new Global();
            AccessableObjects accessableObjects = new(new(), new(NamespaceInfo.NamespaceIntend.nonedef, ""), new());

            accessableObjects.accessableVars.Add("promisetestvar", new Var(new VarConstruct(VarConstruct.VarType.num, "promisetestvar"), new(Value.ValueType.num, "")));
            Statement.StaticStatement(new(StringProcess.ConvertLineToCommand("promise promiseTestVar {} { makeVar num i; while (i < 6969) { set i (i + 1); }; return i; }", global)), accessableObjects);
            //Assert.AreEqual("", ((Var)accessableObjects.accessableVars["promisetestvar"]).varValueHolder.value.value); //This is the worst way to test, please don't send me to hell :3
            Assert.AreEqual( 6969, ((Var)accessableObjects.accessableVars["promisetestvar"]).VarValue.ObjectValue);
        }
        [Test]
        public static void PromiseOutsideTest()
        {
            Global global = new Global();
            AccessableObjects accessableObjects = new(new(), new(NamespaceInfo.NamespaceIntend.nonedef, ""), new());

            accessableObjects.accessableVars.Add("promisetestvar", new Var(new VarConstruct(VarConstruct.VarType.@string, "promisetestvar"), new(Value.ValueType.@string, "")));
            accessableObjects.accessableVars.Add("outside", new Var(new VarConstruct(VarConstruct.VarType.@string, "outside"), new(Value.ValueType.@string, "notChange")));
             
            Statement.StaticStatement(new(StringProcess.ConvertLineToCommand("promise promiseTestVar {} { while (outside == \"notChange\" ) {}; return outside; }", global)), accessableObjects);
            Thread.Sleep(50);
            ((Var)accessableObjects.accessableVars["outside"]).VarValue.ObjectValue = "change";
            Assert.AreEqual("change", ((Var)accessableObjects.accessableVars["promisetestvar"]).VarValue.ObjectValue);
        }
    }
        [TestFixture]
    public class GeneralCodeTests
    {
        [Test]
        public static void HelloWorldTest()
        {
            


            StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            LoadFile.RunCode("name HelloWorldTest;Type Generic;Start {[Console.WriteLine:\"Hello World!\"];};");
            string consoleOutput = sw.ToString();
            Assert.That(consoleOutput, Contains.Substring("Hello World!"));


        }
        [Test]
        public static void ConsoleReadLineHelloWorldTest()
        {

            StringReader sr = new StringReader("Hello world!");
            StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetIn(sr);
            LoadFile.RunCode("name ConsoleReadLineHelloWorldTest;Type Generic;Start {[Console.WriteLine:[Console.ReadLine]];};");
            string consoleOutput = sw.ToString();
            Assert.That(consoleOutput, Contains.Substring("Hello world!"));


        }
        [Test]
        public static void WhileVarTest()
        {

            StringReader sr = new StringReader("");
            StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetIn(sr);
            LoadFile.RunCode("name ConsoleReadLineHelloWorldTest;Type Generic;Start {makeVar int I; set i 0; while (i < 5){[Console.WriteLine:i] set i (i + 1);}; };");
            string consoleOutput = sw.ToString();
            Assert.AreEqual("0\n1\n2\n3\n4\n", consoleOutput.Replace("\r\n", "\n"));


        }
        [Test]
        public static void ComplexWhileTest()
        {

            StringReader sr = new StringReader("3\nTest");
            StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetIn(sr);
            LoadFile.RunCode("Name ComplexTest;Type Generic;Start {makevar num times;set times [Convert.ToNum:[Console.ReadLine], true];makevar string printText;set printText [Console.ReadLine];[Console.WriteLine:[ComplexTest.DoWhileLoop:printText, times]]return;};function string DoWhileLoop {string printText; num repeatLoop}{makevar num i;if (repeatLoop > 99999){return \"high\";} else {if (repeatLoop < 1){return \"low\";};};while (repeatLoop > i){[Console.WriteLine:(printText + \" | \" + (i + 1) + \" out of \" + repeatLoop)]set i (i + 1);}; return \"success\";};");
            string consoleOutput = sw.ToString();
            Assert.AreEqual("Test | 1 out of 3\nTest | 2 out of 3\nTest | 3 out of 3\nsuccess\n", consoleOutput.Replace("\r\n", "\n"));

        }
        [Test]
        public static void ComplexWhileErrorLowTest()
        {

            StringReader sr = new StringReader("-1\nTest");
            StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetIn(sr);
            LoadFile.RunCode("Name ComplexTest;Type Generic;Start {makevar num times;set times [Convert.ToNum:[Console.ReadLine], true];makevar string printText;set printText [Console.ReadLine];[Console.WriteLine:[ComplexTest.DoWhileLoop:printText, times]]return;};function string DoWhileLoop {string printText; num repeatLoop}{makevar num i;if (repeatLoop > 99999){return \"high\";} else {if (repeatLoop < 1){return \"low\";};};while (repeatLoop > i){[Console.WriteLine:(printText + \" | \" + (i + 1) + \" out of \" + repeatLoop)]set i (i + 1);}; return \"success\";};");
            string consoleOutput = sw.ToString();
            Assert.AreEqual("low\n", consoleOutput.Replace("\r\n", "\n"));

        }
        [Test]
        public static void ComplexWhileErrorHighTest()
        {

            StringReader sr = new StringReader("9999999999999999\nTest");
            StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetIn(sr);
            LoadFile.RunCode("Name ComplexTest;Type Generic; Start {makevar num times;set times [Convert.ToNum:[Console.ReadLine], true];makevar string printText;set printText [Console.ReadLine];[Console.WriteLine:[ComplexTest.DoWhileLoop:printText, times]]return;};function string DoWhileLoop {string printText; num repeatLoop}{makevar num i;if (repeatLoop > 99999){return \"high\";} else {if (repeatLoop < 1){return \"low\";};};while (repeatLoop > i){[Console.WriteLine:(printText + \" | \" + (i + 1) + \" out of \" + repeatLoop)]set i (i + 1);}; return \"success\";};");
            string consoleOutput = sw.ToString();
            Assert.AreEqual("high\n", consoleOutput.Replace("\r\n", "\n"));

        }
        [Test]
        public static void LoopTest()
        {

            StringReader sr = new StringReader("");
            StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetIn(sr);
            LoadFile.RunCode("Name LoopTest;Type Generic;Start {makevar int i;set i 0; while (i == 50 !){set i (i + 1); loop; [Console.WriteLine:\"Seems like looping didn't work...\"] };};");
            string consoleOutput = sw.ToString();
            Assert.AreEqual("", consoleOutput);

        }
        [Test]
        public static void LinkTest()
        {
            Assert.AreEqual(15, LoadFile.RunCode("Name LinkTest;Type Generic;Start {makevar num link; makevar num linkTo; set link 10.0; set linkTo 15.0; link link linkTo; return link;};").ObjectValue);
        }
        [Test]
        public static void ListTest()
        {
            Assert.AreEqual("RandomItem!", LoadFile.RunCode("Name ListTest;Type Generic;Start {makevar list randomList; add randomList \"RandomItem\"; add randomList \"AnotherRandomItem\"; add randomList \"!\"; return (($randomList 0) + ($randomList 2));};").ObjectValue);
        }
        [Test]
        public static void NestedListTest()
        {
            Assert.AreEqual("It worked!", LoadFile.RunCode("Name NestedListTest;Type Generic;Start {makevar list randomList; add randomList \"RandomItem\"; add randomList \"AnotherRandomItem\"; add randomList \"!\"; makeVar list insideList; add insideList \"It worked!\"; add randomList insideList; return randomList 3 0;};").ObjectValue);
        }
        [Test]
        public static void SetListTest()
        {
            Assert.AreEqual("It worked!", LoadFile.RunCode("Name SetListTest;Type Generic;Start {makevar list randomList; add randomList \"RandomItem\"; add randomList \"AnotherRandomItem\"; add randomList \"!\"; makeVar list insideList; add insideList \"It worked!\"; setList randomList 2 \"It \" ; add randomList insideList; setList randomList 3 0 \"worked!\"; return (($randomList 2) + ($randomList 3 0));};").ObjectValue);
        }
       
    }


}
