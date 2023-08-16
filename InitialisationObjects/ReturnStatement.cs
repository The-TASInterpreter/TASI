using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.LangCoreHandleInterface;

namespace TASI.InitialisationObjects
{
    public class ReturnStatement : StatementInput
    {
        public IReturnStatementHandler returnStatementHandler;

        public ReturnStatement(string statementName, List<List<StatementInputType>> possibleInputs, IReturnStatementHandler returnStatementHandler) : base(statementName, possibleInputs, true)
        {
            this.returnStatementHandler = returnStatementHandler;
        }
    }
}
