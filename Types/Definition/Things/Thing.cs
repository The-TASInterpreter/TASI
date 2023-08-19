using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.Types.Definition.Visibility;

namespace TASI.Types.Definition.Things
{
    public abstract class Thing
    { 
        public abstract bool isStatic { get; }
        public abstract string actualType { get; }
        public readonly string name;

        public const string CONSTRUCTOR = "constructor";

        public static readonly string[] reservedNames = new string[]
        {
            CONSTRUCTOR
        };

        public Thing(string name, bool allowReservedNames = false)
        {
            this.name = name.ToLower();
            if (reservedNames.Contains(this.name) ) 
            {
                throw new CodeSyntaxException($"The name {this.name} is reserved for internal purposes");
            }
        }
    }
}
