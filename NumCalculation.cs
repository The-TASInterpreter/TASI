namespace Text_adventure_Script_Interpreter
{
    internal class NumCalculation
    {
        public static char[] operators = { '+', '-', '*', '/', '%' };

        public static Var DoNumCalculation(Command command)
        {
            if (command.commandType != Command.CommandTypes.NumCalculation) throw new Exception("Internal: This method only deals with NumCalculations");
            //Grab tokens
            string value0 = "";
            string value1 = "";
            int valuePoint = 0;

            foreach (char c in command.commandText)
            {

            }

            return null;
        }



        public static Var Add(Var var0, Var var1)
        {
            if ((var0.varDef.varType == VarDef.evarType.Num || var0.varDef.varType == VarDef.evarType.Bool) && (var1.varDef.varType == VarDef.evarType.Num || var1.varDef.varType == VarDef.evarType.Bool))
                return new Var(new(VarDef.evarType.Num, ""), true, var0.numValue + var1.numValue);
            if (var0.varDef.varType == VarDef.evarType.String || var1.varDef.varType == VarDef.evarType.String)
                return new Var(new(VarDef.evarType.String, ""), true, Convert.ToString(var0.objectValue) + Convert.ToString(var1.objectValue));

            return null;
        }
    }
}
