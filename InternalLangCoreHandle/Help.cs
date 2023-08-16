using System.Xml.Linq;
using TASI.RuntimeObjects.FunctionClasses;
using TASI.RuntimeObjects.VarClasses;

namespace TASI.InternalLangCoreHandle
{
    internal class Help
    {
        public static void ListFunctionArguments(Function function)
        {
            Console.WriteLine($"Help for methd: {function.funcName}");
            Console.WriteLine($"This function is part of the \"{function.parentNamespace.Name}\" parent. You can use the syntax \"Listm <function location string>;\". For this function it would be:\nListm \"{function.functionLocation}\";");
            Console.WriteLine($"Accepted arguments for this function are: ");
            foreach (List<VarConstruct> arguments in function.functionArguments)
            {

                if (arguments.Count == 0)
                    Console.WriteLine($"\t[{function.functionLocation}]");
                else
                    Console.Write($"\t[{function.functionLocation}:");
                for (int i = 0; i < arguments.Count; i++)
                {
                    VarConstruct var = arguments[i];
                    if (i + 1 == arguments.Count)
                        Console.WriteLine($"<{VarConstruct.VarType.all}> {VarConstruct.VarType.all}]");
                    else
                        Console.Write($"<{VarConstruct.VarType.all}> {VarConstruct.VarType.all},");
                }
            }

        }

        public static void ListFunctionsOfNamespace(string location, Global global)
        {
            Console.WriteLine($"Subfunctions of {location}:");
            Console.WriteLine(ListFunctions(FunctionCall.FindNamespaceByName(location, global.Namespaces, true).namespaceFuncitons));


        }

        public static void ListLocation(string location, Global global)
        {
            if (location.Split('.').Length == 1)
            {
                ListFunctionsOfNamespace(location, global);
            }
            else
            {
                ListSubfunctionsOfFunction(location, global);
            }
        }

        public static void ListSubfunctionsOfFunction(string location, Global global)
        {
            Console.WriteLine($"Subfunctions of {location}:");
            Console.WriteLine(ListFunctions(FunctionCall.FindFunctionByPath(location, global.Namespaces, true, null).subFunctions));
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
