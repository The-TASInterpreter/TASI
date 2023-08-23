using TASI.RuntimeObjects.FunctionClasses;
using TASI.Types.Definition.Things;

namespace TASI.Types.Definition
{
    public class TypeDef
    {

        public bool IsInternal
        {
            get
            {
                return parentNamespace.namespaceIntend == NamespaceInfo.NamespaceIntend.@internal;
            }
        }

        public NamespaceInfo parentNamespace;
        public List<Thing> things;
        public List<Implementation> implementations = new();
        //public List<Thing>? originalThings;
        //public List<TypeDef> inheritedTypes;
        //public List<TypeDef> directBaseTypes;
        public Constructor? constructor;
        public bool isSimpleType;

        public InstantiationType instantiationType;
        string typeName;

        public string GetFullName
        {
            get
            {
                return $"{parentNamespace.Name}.{typeName}";
            }
        }


        /*
        public bool CanBeUsedAs(TypeDef typeDef)
        {
            if (inheritedTypes.Contains(typeDef))
                return true;
            else if (typeDef == this)
                return true;
            else
                return false;
        }
        */
        public TypeDef(string typeName, NamespaceInfo parentNamespace, List<Thing> things, bool isSimpleType, InstantiationType instantiationType, Constructor? constructor)
        {
            this.parentNamespace = parentNamespace;
            this.things = things;
            //this.directBaseTypes = directBaseTypes;
            this.isSimpleType = isSimpleType;
            this.instantiationType = instantiationType;
            this.typeName = typeName;
            this.constructor = constructor;
            //inheritedTypes = GetInheritedTypesOfBaseType();


        }

        public TypeDef(string typeName, NamespaceInfo parentNamespace, List<Thing> things, bool isSimpleType)
            : this(typeName, parentNamespace, things, isSimpleType, InstantiationType.framework, null)

        {


        }


        /*
        public List<TypeDef> GetInheritedTypesOfBaseType()
        {

            List<TypeDef> result = new();

            result.AddRange(directBaseTypes);
            foreach (TypeDef typeDef in directBaseTypes)
            {
                result.AddRange(typeDef.inheritedTypes.Where(x => !result.Contains(x)));
            }
            return result;

        }
        */

        public enum InstantiationType
        {
            framework, //C# equivalent of abstract class
            blueprint, //C# equivalent of interface
            normal
        }
    }
}
