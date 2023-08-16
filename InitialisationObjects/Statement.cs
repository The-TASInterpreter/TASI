using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.LangCoreHandleInterface;

namespace TASI.InitialisationObjects
{
    public class Statement : StatementInput
    {
        public IStatementHandler statementHandler;
        public Statement(string statementName, List<List<StatementInputType>> possibleInputs, IStatementHandler statementHandler) : base(statementName, possibleInputs, false)
        {
            this.statementHandler = statementHandler;
        }


    }
}
