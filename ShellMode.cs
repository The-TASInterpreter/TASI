﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.InternalLangCoreHandle;
using TASI.LangCoreHandleInterface;
using TASI.RuntimeObjects;
using TASI.RuntimeObjects.FunctionClasses;
using TASI.Token;
using static TASI.Command;

namespace TASI
{
    public class ShellStatementHandler : ShellMode, IStatementHandler
    {
        public ShellStatementHandler(Global global) : base(global)
        {
            statementHandler = this;

        }

        public Value? HandleStatement(List<Command> inputStatement, AccessableObjects accessableObjects)
        {
            switch (inputStatement[0].commandText)
            {
                case "clear":
                    Console.Clear();
                    return null;
                case "exit":
                    exitRequest = true;
                    return null;
                default:
                    throw new InternalInterpreterException();
            }
        }

        internal void InitShell(Global global)
        {
            global.AllNormalStatements.Add("clear", new("clear", new() { new() }, statementHandler));
            global.AllNormalStatements.Add("exit", new("exit", new() { new() }, statementHandler));

        }
    }
    public class ShellMode
    {
        protected IStatementHandler statementHandler = null!;
        protected bool exitRequest = false;
        public NamespaceInfo currentNamespace = null!;
        protected Global global;
        public ShellMode(Global global)
        {
            this.global = global;
        }
        public IEnumerable<Command> GetShell()
        {


            while (!exitRequest)
            {

                string input = Console.ReadLine() ?? "";
                foreach (Command command in Tokeniser.CallTokeniseInput(input, global, -1))
                {
                    global.CurrentLine = command.commandLine;
                    if (command.commandType == CommandTypes.FunctionCall) command.functionCall.SearchCallFunction(currentNamespace, global);
                    else if (command.commandType == CommandTypes.Calculation) command.calculation.InitFunctions(currentNamespace, global);
                    else if (command.commandType == CommandTypes.CodeContainer) command.initCodeContainerFunctions(currentNamespace, global);
                    yield return command;
                }
            }
        }
    }
}