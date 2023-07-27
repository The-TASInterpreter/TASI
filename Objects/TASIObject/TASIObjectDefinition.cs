namespace TASI
{
    public class TASIObjectDefinition
    {
        public string objectType;
        public List<FieldDefinition> fields = new();
        public List<TASIObjectDefinition> baseTypes = new();

    }

    public class FieldLayout
    {
        public List<Field> fields = new();
        public FieldLayout Clone()
        {
            return null;
        }
    }
    public class Field
    {
        public FieldDefinition fieldDefinition;
        private Value simple;
        private TASIObjectInstance pointer;

        public Value GetValue()
        {
            return null;
        }
    }
}
