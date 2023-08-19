using TASI.RuntimeObjects.FunctionClasses;
using TASI.Types.Definition.Field;

namespace TASI.Types.Definition
{
    public class TypeDef
    {
        public NamespaceInfo parentNamespace;
        public List<FieldDef> fields;
        public List<Method> methods;
        public List<TypeDef> inheritedTypes;
        public List<TypeDef> directBaseTypes;
        public Method constructor;
        public bool isSimpleType;

        public InstantiationType instantiationType;
        string typeName;

        public string GetFullName
        {
            get
            {
                return $"<T>{parentNamespace.Name}.{typeName}";
            }
        }

        public bool CanBeUsedAs(TypeDef typeDef)
        {
            if (inheritedTypes.Contains(typeDef))
                return true;
            else
                return false;
        }

        public TypeDef(string typeName, NamespaceInfo parentNamespace, List<FieldDef> fields, List<Method> methods, List<TypeDef> directBaseTypes, bool isSimpleType, InstantiationType instantiationType)
        {
            this.parentNamespace = parentNamespace;
            this.fields = fields;

            this.directBaseTypes = directBaseTypes;
            this.isSimpleType = isSimpleType;
            this.instantiationType = instantiationType;
            this.typeName = typeName;
            this.methods = methods;

            inheritedTypes = GetInheritedTypesOfBaseType();


        }

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

        public enum InstantiationType
        {
            framework, //C# equivalent of abstract class
            blueprint, //C# equivalent of interface
            normal
        }
    }
}
