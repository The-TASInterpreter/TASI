using System.Xml.Linq;

namespace TASI
{
    internal class Help
    {
        public static void ListMethodArguments(Method method)
        {
            Console.WriteLine($"Help for methd: {method.funcName}");
            Console.WriteLine($"This method is part of the \"{method.parentNamespace.Name}\" parent. You can use the syntax \"Listm <method location string>;\". For this method it would be:\nListm \"{method.methodLocation}\";");
            Console.WriteLine($"Accepted arguments for this method are: ");
            foreach (List<VarDef> arguments in method.methodArguments)
            {

                if (arguments.Count == 0)
                    Console.WriteLine($"\t[{method.methodLocation}]");
                else
                    Console.Write($"\t[{method.methodLocation}:");
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

        public static void ListMethodsOfNamespace(string location)
        {
            Console.WriteLine($"Submethods of {location}:");
            Console.WriteLine(ListMethods(MethodCall.FindNamespaceByName(location, Global.Namespaces, true).namespaceMethods));


        }

        public static void ListLocation(string location)
        {
            if (location.Split('.').Length == 1)
            {
                ListMethodsOfNamespace(location);
            }
            else
            {
                ListSubmethodsOfMethod(location);
            }
        }

        public static void ListSubmethodsOfMethod(string location)
        {
            Console.WriteLine($"Submethods of {location}:");
            Console.WriteLine(ListMethods(MethodCall.FindMethodByPath(location, Global.Namespaces, true).subMethods));
        }

        public static void ListNamespaces(List<NamespaceInfo> namespaces)
        {
            if (namespaces.Count == 0) Console.WriteLine("\t<There are none>");
            foreach (NamespaceInfo ns in namespaces)           
                Console.WriteLine("\t" + ns.Name);
            return;
            
        }

        public static string ListMethods(List<Method> methods)
        {
            if (methods.Count == 0) return "\t <There are none>";
            string result = "";
            foreach (Method m in methods)
            {
                result += "\t" + m.methodLocation + "\n";
            }
            return result;
        }
    }
}
