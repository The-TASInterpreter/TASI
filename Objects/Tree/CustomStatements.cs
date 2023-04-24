using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI
{
    public class CustomStatement
    {
        public enum StatementType
        {
            returnstatement, statement
        }
        public StatementType statementType;
        public TreeElement treeElement;
        public string statementName;


        public CustomStatement(StatementType statementType, TreeElement treeElement, string statementName)
        {
            this.statementType = statementType;
            this.treeElement = treeElement;
            this.statementName = statementName;
        }
    }
}
