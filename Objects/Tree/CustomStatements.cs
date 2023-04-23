using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI.Objects.Tree
{
    internal class CustomStatement
    {
        public enum StatementType
        {
            returnstatement, statement
        }
        public StatementType statementType;
        public TreeElement treeElement;


        public CustomStatement(StatementType statementType, TreeElement treeElement)
        {
            this.statementType = statementType;
            this.treeElement = treeElement;

        }
    }
}
