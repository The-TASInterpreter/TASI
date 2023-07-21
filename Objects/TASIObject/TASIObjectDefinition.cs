namespace TASI
{
    public class TASIObjectDefinition
    {
        public string objectType;
        public List<string> simpleFieldNames;
        public List<string> pointerFieldNames;
    }

    public class Field
    {
        enum FieldType
        {
            @public, @private
        }
        enum ValueType
        {
            simple, pointer
        }
        Value simple;
        TASIObjectInstance pointer;
    }
}
