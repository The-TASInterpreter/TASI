using System.Text;
using TASI.InternalLangCoreHandle;
using TASI.Types.Definition;
using TASI.Types.Definition.Field;
using TASI.Types.Definition.Things;
using TASI.Types.Instance.Field;

namespace TASI.Types.Instance
{
    public class TypeInstance
    {
        public readonly TypeDef typeDef;

        public Dictionary<string, FieldInstance> fieldInstances = new();
        public Dictionary<string, Thing> staticThings = new();

        private void GenerateSkeleton()
        {
            foreach (Thing thing in typeDef.things)
            {
                if (thing is FieldDef fieldDef)
                {
                    fieldInstances.Add(fieldDef.name, new(fieldDef, null));
                }
                else
                {
                    staticThings.Add(thing.name, thing);
                }
            }
        }


        public TypeInstance(List<TypeInstance> constructorArgs, TypeDef typeDef, AccessableObjects accessableObjects)
        {
            //Generating instance architecture
            this.typeDef = typeDef;
            GenerateSkeleton();
            //Call constructor
            AccessableObjects inMethod = new(new()
            {
                {"self", new(typeDef, this) }
            }, typeDef.parentNamespace, accessableObjects.global);
            Overload correctConstructor = Overload.GetCorrectOverload(typeDef.constructor.overloads, constructorArgs) ?? throw new CodeSyntaxException($"A constructor for the type \"{typeDef.GetFullName}\" doesn't exist with the provided input values.");
            TypeInstance returnValue; //This should be void
            if (typeDef.IsInternal)
                returnValue = correctConstructor.methodHandler.Invoke(constructorArgs, inMethod, this);
            else
                returnValue = InterpretMain.InterpretNormalMode(correctConstructor.commands, inMethod);
            if (returnValue.typeDef != accessableObjects.global.TYPE_DEF_VOID) throw new CodeSyntaxException($"The constructor for \"{typeDef.GetFullName}\" with the input arguments \"{correctConstructor.GetCallName}\" returned not the expected type of void.");

        }


        public TypeInstance(TypeDef typeDef, object simpleTypeValue)
        {
            if (!typeDef.isSimpleType)
                throw new InternalInterpreterException("Calling constructor for simple type with non-simple type def");

            this.typeDef = typeDef;
            GenerateSkeleton();


            this.simpleTypeValue = simpleTypeValue;
        }


        private object? simpleTypeValue;



        public static List<TypeDef> ConverToTypeDef(List<TypeInstance> instances)
        {
            List<TypeDef> ret = new List<TypeDef>();
            foreach (var instance in instances)
            {
                ret.Add(instance.typeDef);
            }
            return ret;
        }

        public static string GetDescriberOfInput(List<TypeInstance> instances)
        {
            StringBuilder sb = new StringBuilder("(");
            for (int i = 0; i < instances.Count; i++)
            {
                if (i != 0)
                    sb.Append(", ");
                sb.Append(instances[i].typeDef.GetFullName);
            }
            return sb.ToString();
        }
    }
}
