using TASI.Objects.TASIObject;

namespace TASI
{
    public class FieldDefinition
    {
        public enum FieldVisability
        {
            @public, @private
        }
        public enum FieldType
        {
            simple, pointer, method
        }
        public FieldVisability visability;
        public FieldType type;
        public Method? method;
        public TASIObjectDefinition? pointer;
        public Value.ValueType? simpleType;
        public string name;
        /// <summary>
        /// This will assume that the field type is a Method
        /// </summary>
        /// <param name="fieldVisability"></param>
        /// <param name="method"></param>
        public FieldDefinition(FieldVisability visability, Method method, string name)
        {
            this.method = method;
            this.visability = visability;
            type = FieldType.method;
            this.name = name;
        }

        /// <summary>
        /// This will assume that the field type is a simple
        /// </summary>
        /// <param name="visability"></param>
        /// <param name="simpleType"></param>
        /// <param name="name"></param>
        public FieldDefinition(FieldVisability visability, Value.ValueType simpleType, string name)
        {
            this.simpleType = simpleType;
            this.visability = visability;
            type = FieldType.simple;
            this.name = name;
        }

        public FieldDefinition(FieldVisability visability, FieldType type, string name)
        {
            this.visability = visability;
            this.type = type;
            this.name = name;
        }
        /// <summary>
        /// This will assume that the field type is a pointer
        /// </summary>
        /// <param name="visability"></param>
        /// <param name="pointer"></param>
        public FieldDefinition(FieldVisability visability, TASIObjectDefinition pointer, string name)
        {
            this.visability = visability;
            this.pointer = pointer;
            type = FieldType.pointer;
            this.name = name;
        }
    }
}
