namespace TASI
{
    public class Method
    {
        public TASIObjectDefinition parentType;
        public NamespaceInfo parentNamespace;
        public string methodName;

        public VarConstruct returnType;
        public List<List<VarConstruct>> methodArguments;
        public List<List<Command>> methodCode;
        public string MethodLocation
        {
            get
            {
                return $"{parentNamespace.Name}.{parentType.objectType}.{methodName}";
            }
        }

        public Method(TASIObjectDefinition parentType, NamespaceInfo parentNamespace, string methodName, VarConstruct returnType, List<List<VarConstruct>> methodArguments, List<List<Command>> methodCode)
        {
            this.parentType = parentType ?? throw new ArgumentNullException(nameof(parentType));
            this.parentNamespace = parentNamespace ?? throw new ArgumentNullException(nameof(parentNamespace));
            this.methodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
            this.returnType = returnType;
            this.methodArguments = methodArguments ?? throw new ArgumentNullException(nameof(methodArguments));
            this.methodCode = methodCode ?? throw new ArgumentNullException(nameof(methodCode));
        }
    }
}
