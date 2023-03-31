namespace TASI
{
    internal class NumCalculation
    {


        public static Var DoNumCalculation(Command command)
        {
            if (command.commandType != Command.CommandTypes.NumCalculation) throw new Exception("Internal: This method only deals with NumCalculations");
            //Grab tokens

            CalculationType calculation = new(command.commandText, false, true, false);



            return calculation.CalculateValue();
        }



        public static Var Add(Var var0, Var var1)
        {
            if (var0.isNumeric && var1.isNumeric)
                //If both are either num or bool (Its the same)
                return new Var(new(VarDef.evarType.Num, ""), true, var0.numValue + var1.numValue);
            if (var0.varDef.varType == VarDef.evarType.String || var1.varDef.varType == VarDef.evarType.String)
                //Checking if either are string. (If one or both are, convert both to strings and attatch.
                return new Var(new(VarDef.evarType.String, ""), true, Convert.ToString(var0.objectValue) + Convert.ToString(var1.objectValue));
            throw new Exception($"Cant add {var0.varDef.varType} with {var1.varDef.varType}.");
        }
        public static Var Sub(Var var0, Var var1)
        {
            if (var0.isNumeric && var1.isNumeric)
                return new Var(new(VarDef.evarType.Num, ""), true, var0.numValue - var1.numValue);
            throw new Exception($"Can't sub {var0.varDef.varType} with {var1.varDef.varType}.");
        }

        public static Var Mul(Var var0, Var var1)
        {
            if (var0.isNumeric && var1.isNumeric)
            {
                return new Var(new(VarDef.evarType.Num, ""), true, var0.numValue * var1.numValue);
            }
            throw new Exception($"Cant mul {var0.varDef.varType} with {var1.varDef.varType}.");
        }
        public static Var Div(Var var0, Var var1)
        {
            if (var0.isNumeric && var1.isNumeric)
            {
                if (var1.numValue == 0) throw new Exception("Cant devide by 0");
                return new Var(new(VarDef.evarType.Num, ""), true, var0.numValue / var1.numValue);
            }
            throw new Exception($"Cant div {var0.varDef.varType} with {var1.varDef.varType}.");
        }
        public static Var Mod(Var var0, Var var1)
        {
            if (var0.isNumeric && var1.isNumeric)
            {
                return new Var(new(VarDef.evarType.Num, ""), true, var0.numValue % var1.numValue);
            }
            throw new Exception($"Cant mod {var0.varDef.varType} with {var1.varDef.varType}.");
        }

        public static Var Root(Var var0, Var var1)
        {
            if (var0.isNumeric && var1.isNumeric)
            {
                return new Var(new(VarDef.evarType.Num, ""), true, Math.Pow(var0.numValue ?? throw new Exception("Internal: numValue of arg 0 is null"), 1 / var1.numValue ?? throw new Exception("Internal: numValue of arg 1 is null")));
                // Math.Pow(X, (1 / N)) is the same as Nth root of X
            }
            throw new Exception($"Cant root {var0.varDef.varType} with {var1.varDef.varType}.");
        }
        public static Var Equ(Var var0, Var var1)
        {
            return new Var(new(VarDef.evarType.Bool, ""), true, var0.objectValue.ToString() == var1.objectValue.ToString());
        }
        public static Var Not(Var var0)
        {
            return new Var(new(VarDef.evarType.Bool, ""), true, !var0.getBoolValue);
        }
        public static Var Grt(Var var0, Var var1)
        {
            if (var0.isNumeric && var1.isNumeric)
            {
                return new Var(new(VarDef.evarType.Bool, ""), true, var0.numValue > var1.numValue);
            }
            throw new Exception($"Cant grt {var0.varDef.varType} with {var1.varDef.varType}.");

        }
        public static Var Sml(Var var0, Var var1)
        {
            if (var0.isNumeric && var1.isNumeric)
            {
                return new Var(new(VarDef.evarType.Bool, ""), true, var0.numValue < var1.numValue);
            }
            throw new Exception($"Cant sml {var0.varDef.varType} with {var1.varDef.varType}.");

        }

        public static Var And(Var var0, Var var1)
        {
            return new Var(new(VarDef.evarType.Bool, ""), true, var0.getBoolValue && var1.getBoolValue);
        }

        public static Var Or(Var var0, Var var1)
        {
            return new Var(new(VarDef.evarType.Bool, ""), true, var0.getBoolValue || var1.getBoolValue);
        }


    }


    internal class CalculationType
    {
        public enum Type
        {
            add, sub, mul, div, mod, root, num, calc, str, syx, equ, not, grt, sml, and, or
        }
        public Type type;
        public double? value;
        public string? stringValue;
        public string token;
        private List<CalculationType>? subTokens;
        private Var? varReturn;
        public bool isValue;
        public bool isOperator = false;


        public Var VarReturn
        {
            get
            {
                if (varReturn != null) return varReturn;
                switch (type)
                {
                    case Type.num:
                        varReturn = new(new(VarDef.evarType.Num, ""), true, value);
                        break;
                    case Type.str:
                        varReturn = new(new(VarDef.evarType.String, ""), true, stringValue);
                        break;
                    default:
                        throw new Exception("Internal: Only values can have a var return.");
                }
                return varReturn;
            }
        }
        public List<CalculationType> SubTokens
        {
            get
            {
                if (type != Type.calc) throw new Exception("Internal: Only a numcalc can have subtokens.");
                if (subTokens != null) return subTokens;
                subTokens = new List<CalculationType>();
                foreach (Command command in StringProcess.ConvertLineToCommand(token))
                    subTokens.Add(new(command.commandText, command.commandType == Command.CommandTypes.String, command.commandType == Command.CommandTypes.NumCalculation, command.commandType == Command.CommandTypes.UnknownMethod));
                return subTokens;
            }
        }


        public Var CalculateValue()
        {
            if (type != Type.calc && type != Type.syx) throw new Exception("Internal: Only numcalcs or syntax can return a Value.");
            if (type == Type.syx) return Statement.ReturnStatement(StringProcess.ConvertLineToCommand(token.Remove(0, 1))); // If its not a num calculation but a syntax/statement/(Everywhere I call shit differently), we can skip all this calculating, and 

            CalculationType? calculationNext = null;
            CalculationType calculation;
            Var temp;
            List<Var> values = new List<Var>();
            int skip = 0;
            for (int i = 0; i < SubTokens.Count; i++)
            {
                if (skip > 0)
                {
                    calculationNext = null;
                    skip--;
                    continue;
                }

                if (calculationNext != null && calculationNext.type != SubTokens[i].type)
                    //Calc has already been done last loop because of precalc.
                    calculation = calculationNext;
                else
                    calculation = SubTokens[i];
                if (i + 2 <= SubTokens.Count)
                {

                    calculationNext = SubTokens[i + 1];
                    if (calculationNext.type == Type.syx || calculationNext.type == Type.calc)
                    {
                        temp = calculationNext.CalculateValue();
                        switch (temp.varDef.varType)
                        {
                            case VarDef.evarType.Num or VarDef.evarType.Bool:
                                calculationNext = new CalculationType(temp.numValue.ToString(), false, false, false);
                                break;
                            case VarDef.evarType.Void:
                                throw new Exception("Can't calculate with a void type.");
                            case VarDef.evarType.String:
                                calculationNext = new CalculationType(temp.stringValue, true, false, false);
                                break;
                            default:
                                throw new Exception("Internal: Unimplemented var type.");
                        }
                    }
                }
                else
                    calculationNext = null;
                switch (calculation.type)
                {
                    case Type.num or Type.str:
                        values.Add(calculation.VarReturn);
                        break;
                    case Type.add:
                        if (values.Count != 2)
                        {
                            if (calculationNext != null && calculationNext.isValue && values.Count == 1)
                            {
                                skip = 1;
                                values.Add(calculationNext.VarReturn);
                            }
                            else
                                throw new Exception("The add operator doesnt support less or more than two values.");
                        }

                        temp = NumCalculation.Add(values[0], values[1]);
                        values.Clear();
                        values.Add(temp);
                        break;
                    case Type.sub:
                        if (values.Count != 2)
                        {
                            if (calculationNext != null && calculationNext.isValue && values.Count == 1)
                            {
                                skip = 1;
                                values.Add(calculationNext.VarReturn);
                            }
                            else
                                throw new Exception("The sub operator doesnt support less or more than two values.");
                        }
                        temp = NumCalculation.Sub(values[0], values[1]);
                        values.Clear();
                        values.Add(temp);
                        break;
                    case Type.mul:
                        if (values.Count != 2)
                        {
                            if (calculationNext != null && calculationNext.isValue && values.Count == 1)
                            {
                                skip = 1;
                                values.Add(calculationNext.VarReturn);
                            }
                            else
                                throw new Exception("The mul operator doesnt support less or more than two values.");
                        }
                        temp = NumCalculation.Mul(values[0], values[1]);
                        values.Clear();
                        values.Add(temp);
                        break;
                    case Type.div:
                        if (values.Count != 2)
                        {
                            if (calculationNext != null && calculationNext.isValue && values.Count == 1)
                            {
                                skip = 1;
                                values.Add(calculationNext.VarReturn);
                            }
                            else
                                throw new Exception("The div operator doesnt support less or more than two values.");
                        }
                        temp = NumCalculation.Div(values[0], values[1]);
                        values.Clear();
                        values.Add(temp);
                        break;
                    case Type.mod:
                        if (values.Count != 2)
                        {
                            if (calculationNext != null && calculationNext.isValue && values.Count == 1)
                            {
                                skip = 1;
                                values.Add(calculationNext.VarReturn);
                            }
                            else
                                throw new Exception("The mod operator doesnt support less or more than two values.");
                        }
                        temp = NumCalculation.Mod(values[0], values[1]);
                        values.Clear();
                        values.Add(temp);
                        break;
                    case Type.root:
                        if (values.Count != 2)
                        {
                            if (calculationNext != null && calculationNext.isValue && values.Count == 1)
                            {
                                skip = 1;
                                values.Add(calculationNext.VarReturn);
                            }
                            else
                                throw new Exception("The root operator doesnt support less or more than two values.");
                        }
                        temp = NumCalculation.Root(values[0], values[1]);
                        values.Clear();
                        values.Add(temp);
                        break;

                    case Type.equ:
                        if (values.Count != 2)
                        {
                            if (calculationNext != null && calculationNext.isValue && values.Count == 1)
                            {
                                skip = 1;
                                values.Add(calculationNext.VarReturn);
                            }
                            else
                                throw new Exception("The equ operator doesnt support less or more than two values.");
                        }
                        temp = NumCalculation.Equ(values[0], values[1]);
                        values.Clear();
                        values.Add(temp);
                        break;
                    case Type.not:
                        if (values.Count != 1)
                        {
                            if (calculationNext != null && calculationNext.isValue && values.Count == 0)
                            {
                                skip = 1;
                                values.Add(calculationNext.VarReturn);
                            }
                            else
                                throw new Exception("The not operator doesnt support less or more than one value.");
                        }
                        temp = NumCalculation.Not(values[0]);
                        values.Clear();
                        values.Add(temp);
                        break;
                    case Type.grt:
                        if (values.Count != 2)
                        {
                            if (calculationNext != null && calculationNext.isValue && values.Count == 1)
                            {
                                skip = 1;
                                values.Add(calculationNext.VarReturn);
                            }
                            else
                                throw new Exception("The equ operator doesnt support less or more than two values.");
                        }
                        temp = NumCalculation.Grt(values[0], values[1]);
                        values.Clear();
                        values.Add(temp);
                        break;
                    case Type.sml:
                        if (values.Count != 2)
                        {
                            if (calculationNext != null && calculationNext.isValue && values.Count == 1)
                            {
                                skip = 1;
                                values.Add(calculationNext.VarReturn);
                            }
                            else
                                throw new Exception("The equ operator doesnt support less or more than two values.");
                        }
                        temp = NumCalculation.Sml(values[0], values[1]);
                        values.Clear();
                        values.Add(temp);
                        break;
                    case Type.and:
                        if (values.Count != 2)
                        {
                            if (calculationNext != null && calculationNext.isValue && values.Count == 1)
                            {
                                skip = 1;
                                values.Add(calculationNext.VarReturn);
                            }
                            else
                                throw new Exception("The equ operator doesnt support less or more than two values.");
                        }
                        temp = NumCalculation.And(values[0], values[1]);
                        values.Clear();
                        values.Add(temp);
                        break;
                    case Type.or:
                        if (values.Count != 2)
                        {
                            if (calculationNext != null && calculationNext.isValue && values.Count == 1)
                            {
                                skip = 1;
                                values.Add(calculationNext.VarReturn);
                            }
                            else
                                throw new Exception("The equ operator doesnt support less or more than two values.");
                        }
                        temp = NumCalculation.Or(values[0], values[1]);
                        values.Clear();
                        values.Add(temp);
                        break;


                    case Type.calc or Type.syx:
                        values.Clear();
                        values.Add(calculation.CalculateValue());
                        break;

                }
            }
            if (values.Count != 1) throw new Exception("Invalid num calculation (Ended up with too many or few tokens)");

            return values[0];

        }

        public CalculationType(string token, bool isString, bool isNumCalc, bool isMethod)
        {
            this.token = token;
            isValue = false;
            if (isString)
            {
                isValue = true;
                type = Type.str;
                stringValue = token;
                return;
            }
            if (isNumCalc)
            {
                if (token.StartsWith("$"))
                    type = Type.syx;
                else
                    type = Type.calc;
                return;
            }
            if (isMethod)
            { //Methods will directly be calculated.
                MethodCall methodCall = new(new(Command.CommandTypes.UnknownMethod, token));
                isValue = true;
                switch (methodCall.callMethod.returnType)
                {
                    case VarDef.evarType.Void:
                        throw new Exception("A void type cant be used in a num calculation.");
                    case VarDef.evarType.Num or VarDef.evarType.Bool:
                        type = Type.num;
                        value = methodCall.DoMethodCall().numValue;
                        break;
                    case VarDef.evarType.String:
                        type = Type.str;
                        stringValue = methodCall.DoMethodCall().stringValue;
                        break;
                    default:
                        throw new Exception("Internal: Unimplemented method return type.");

                }

                return;
            }
            if (double.TryParse(token, out double parsed))
            {
                isValue = true;
                value = parsed;
                type = Type.num;
                return;
            }

            isOperator = true;
            switch (token)
            {
                case "+":
                    type = Type.add;
                    return;
                case "-":
                    type = Type.sub;
                    return;
                case "*":
                    type = Type.mul;
                    return;
                case "/":
                    type = Type.div;
                    return;
                case "%":
                    type = Type.mod;
                    return;
                case "root":
                    type = Type.root;
                    return;
                case "=":
                    type = Type.equ;
                    return;
                case "!":
                    type = Type.not;
                    return;
                case ">":
                    type = Type.grt;
                    return;
                case "<":
                    type = Type.sml;
                    return;
                case "and":
                    type = Type.and;
                    return;
                case "or":
                    type = Type.or;
                    return;
                default:
                    throw new Exception($"\"{token} is neither a number nor an operator, a method or a string.\nIf you want to use syntax, put it in braces and put a $ in front e.g.:(5+($true))");
            }

        }
    }

}
