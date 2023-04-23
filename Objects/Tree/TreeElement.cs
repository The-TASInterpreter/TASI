namespace TASI.Objects.Tree
{
    internal class TreeElement
    {
        public enum ElementType
        {
            Compare,    // |> "SthElse" => [Something];
            Always,     // | [Code]
            Check,      // |§> boolVar => CODE;
            Else,       // |=> code;
            Break       // -;
        }

        public TreeElement(ElementType elementType, CommandLine? commandLine0, CommandLine? doCode, bool isBranch)
        {
            this.elementType = elementType;
            switch (elementType)
            {
                case ElementType.Compare:
                    checkLine = commandLine0;
                    this.doCode = doCode;
                    break;
                case ElementType.Always:
                    this.doCode = doCode;
                    break;
                case ElementType.Else:
                    this.doCode = doCode;
                    break;
                case ElementType.Check:
                    this.doCode = doCode;
                    checkLine = commandLine0;
                    break;


            }
            if (isBranch)
                subBranch = new();
            this.isBranch = isBranch;
        }

        public ElementType elementType;
        public List<TreeElement>? subBranch;
        public bool isBranch;
        public CommandLine? checkLine;
        public CommandLine? doCode;
        public CommandLine? provide;


    }
}
