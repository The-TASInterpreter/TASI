using System.Xml.Linq;

namespace TASI
{
    internal class Help
    {
        public static void ListFunctionArguments(Function function)
        {
            Console.WriteLine($"Help for methd: {function.funcName}");
            Console.WriteLine($"This function is part of the \"{function.parentNamespace.Name}\" parent. You can use the syntax \"Listm <function location string>;\". For this function it would be:\nListm \"{function.functionLocation}\";");
            Console.WriteLine($"Accepted arguments for this function are: ");
            foreach (List<VarDef> arguments in function.functionArguments)
            {

                if (arguments.Count == 0)
                    Console.WriteLine($"\t[{function.functionLocation}]");
                else
                    Console.Write($"\t[{function.functionLocation}:");
                for (int i = 0; i < arguments.Count; i++)
                {
                    VarDef var = arguments[i];
                    if (i + 1 == arguments.Count)
                        Console.WriteLine($"<{var.varType}> {var.varName}]");
                    else
                        Console.Write($"<{var.varType}> {var.varName},");
                }
            }

        }

        public static void ListFunctionsOfNamespace(string location)
        {
            Console.WriteLine($"Subfunctions of {location}:");
            Console.WriteLine(ListFunctions(FunctionCall.FindNamespaceByName(location, Global.Namespaces, true).namespaceFuncitons));


        }

        public static void ListLocation(string location)
        {
            if (location.Split('.').Length == 1)
            {
                ListFunctionsOfNamespace(location);
            }
            else
            {
                ListSubfunctionsOfFunction(location);
            }
        }

        public static void ListSubfunctionsOfFunction(string location)
        {
            Console.WriteLine($"Subfunctions of {location}:");
            Console.WriteLine(ListFunctions(FunctionCall.FindFunctionByPath(location, Global.Namespaces, true, null).subFunctions));
        }

        public static void ListNamespaces(List<NamespaceInfo> namespaces)
        {
            if (namespaces.Count == 0) Console.WriteLine("\t<There are none>");
            foreach (NamespaceInfo ns in namespaces)           
                Console.WriteLine("\t" + ns.Name);
            return;
            
        }

        public static string ListFunctions(List<Function> functions)
        {
            if (functions.Count == 0) return "\t <There are none>";
            string result = "";
            foreach (Function m in functions)
            {
                result += "\t" + m.functionLocation + "\n";
            }
            return result;
        }
    }
}
