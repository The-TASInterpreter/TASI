namespace DataTypeStore
{
    public class DirectValue
    {
        public string name;
        public string value;
        public DirectValue(string name, string value, bool decodeValue)
        {
            this.name = name;
            if (decodeValue)
                this.value = DirectValueClearify.DecodeInvalidCharCode(value);
            else
                this.value = value;
        }
    }

}