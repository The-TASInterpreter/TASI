namespace TASI
{

    internal class NumCalcTest
    {
        public double? expected;
        public string calc;
        public bool expectedFail;
        public NumCalcTest(double? expected, string calc, bool expectedFail)
        {
            this.expected = expected;
            this.calc = calc;
            this.expectedFail = expectedFail;
        }
        public void DoTest(int testIdx)
        {
            try
            {
                double? testValue = Calculation.DoCalculation(new(Command.CommandTypes.Calculation, calc), new(new(), new(NamespaceInfo.NamespaceIntend.nonedef, ""))).numValue;
                if (testValue == expected)
                    Console.WriteLine($"NumCalcTest {testIdx}: Passed");
                else
                    Console.WriteLine($"NumCalcTest {testIdx}: Failed with {testValue}; Expected {expected}");
            }
            catch (Exception ex)
            {
                if (expectedFail)

                    Console.WriteLine($"NumCalcTest {testIdx}: Passed; Successfully failed with exeption:\n{ex.Message}\n(I always wanted to say that).");
                else
                    Console.WriteLine($"NumCalcTest {testIdx}: Failed because of exception: {ex.Message}");
            }

        }
    }

    internal class Tests
    {
        public static void NumCalcTests()
        {
            NumCalcTest[] tests = {
                new(4.5, "((4 + 6) * 2 - (3 - 2)) / (1 + 1) % 5", false),
                new(0, "(4 + 6) * (3 - 2) / (1 + 1) % 5", false),
                new(20, "4 + 6 * 2", false),
                new(42, "10 + 30 + ( - (5 - 7))", false),
                new(1, "1", false),
                new(1, "($true)", false),
                new(0, "($false)", false),
                new(0, "\"A\" - \"B\"", true),
                new(1, "(3 > 2) and (4 < 5) or (!(6 == 7))", false),
                new(0, "\"4\" == 4 4", false),
                new(1, "4 4 == 4 4", false),
                new(1, "\"4\" \"4\" == \"4\" \"4\"", false),
                new(0, "\"4\" \"1\" == \"4\" \"4\"", false),
                new(1, "\"string\" 4 = \"4\" 4" , false),
                new(0, "\"bool\" 4 = \"4\" 4" , false),
                new(1, "\"num\" 4 = \"4\" 4" , false),
                new(-21, "-21" , false),
                new(-21, "- 21" , false) //Yes, there's a difference
            };
            for (int i = 0; i < tests.Length; i++)
            {
                tests[i].DoTest(i);
            }


        }
    }
}
