﻿namespace TASI
{
    public class TreeElement
    {

        public Value? SimulateTree(Value? provided, AccessableObjects accessableObjects, out bool foundMatch)
        {
            switch (elementType)
            {
                case ElementType.Compare:
                    Value valueOfCheck = Statement.GetValueOfCommandLine(checkLine, accessableObjects);
                    if (provided == null) throw new CodeSyntaxException("You need to provide a Value to use a Compare branch.");
                    if (provided.valueType == valueOfCheck.valueType && provided.ObjectValue.Equals(valueOfCheck.ObjectValue))
                    {
                        foundMatch = true;
                        if (isBranch)
                        {
                            bool foundInternalMatchTracker = false;
                            bool foundInternalMatch = false;
                            foreach (TreeElement treeElement in subBranch)
                            {
                                Value returnedValue = null;
                                if (treeElement.elementType == ElementType.Break && foundInternalMatchTracker)
                                    return null;
                                if (!(treeElement.elementType == ElementType.Else && foundInternalMatchTracker))
                                    if (provide == null)
                                        returnedValue = treeElement.SimulateTree(null, accessableObjects, out foundInternalMatch);
                                    else
                                        returnedValue = treeElement.SimulateTree(Statement.GetValueOfCommandLine(provide, accessableObjects), accessableObjects, out foundInternalMatch);
                                if (foundInternalMatch) foundInternalMatchTracker = true;
                                if (returnedValue != null) return returnedValue;
                            }
                        }
                        else
                        {
                            return InterpretMain.InterpretNormalMode(doCode.commands, accessableObjects);
                        }
                    }
                    foundMatch = false;
                    return null;
                case ElementType.Always or ElementType.Else:
                    foundMatch = false;
                    if (isBranch)
                    {
                        bool foundInternalMatchTracker = false;
                        bool foundInternalMatch = false;
                        foreach (TreeElement treeElement in subBranch)
                        {
                            Value returnedValue = null;
                            if (treeElement.elementType == ElementType.Break && foundInternalMatchTracker)
                                return null;
                            if (!(treeElement.elementType == ElementType.Else && foundInternalMatchTracker))
                                if (provide == null)
                                    returnedValue = treeElement.SimulateTree(null, accessableObjects, out foundInternalMatch);
                                else
                                    returnedValue = treeElement.SimulateTree(Statement.GetValueOfCommandLine(provide, accessableObjects), accessableObjects, out foundInternalMatch);
                            if (foundInternalMatch) foundInternalMatchTracker = true;
                            if (returnedValue != null) return returnedValue;
                        }
                    }
                    else
                    {
                        return InterpretMain.InterpretNormalMode(doCode.commands, accessableObjects);
                    }
                    return null;
                case ElementType.Check:
                    valueOfCheck = Statement.GetValueOfCommandLine(checkLine, accessableObjects);
                    if (valueOfCheck.GetBoolValue)
                    {
                        foundMatch = true;
                        if (isBranch)
                        {
                            bool foundInternalMatchTracker = false;
                            bool foundInternalMatch = false;
                            foreach (TreeElement treeElement in subBranch)
                            {
                                Value returnedValue = null;
                                if (treeElement.elementType == ElementType.Break && foundInternalMatchTracker)
                                    return null;
                                if (!(treeElement.elementType == ElementType.Else && foundInternalMatchTracker))
                                    if (provide == null)
                                        returnedValue = treeElement.SimulateTree(null, accessableObjects, out foundInternalMatch);
                                    else
                                        returnedValue = treeElement.SimulateTree(Statement.GetValueOfCommandLine(provide, accessableObjects), accessableObjects, out foundInternalMatch); if (foundInternalMatch) foundInternalMatchTracker = true;
                                if (returnedValue != null) return returnedValue;
                            }
                        }
                        else
                        {
                            return InterpretMain.InterpretNormalMode(doCode.commands, accessableObjects);
                        }
                    }
                    foundMatch = false;
                    return null;
                default:
                    throw new InternalInterpreterException($"Tried to execute a {elementType}-type");
            }
        }

        public enum ElementType
        {
            Compare,    // |> "SthElse" => [Something];
            Always,     // | [Code]
            Check,      // |§> boolVar => CODE;
            Else,       // |=> code;
            Break       // -;
        }

        public TreeElement(ElementType elementType, CommandLine? commandLine0, CommandLine? doCode, bool isBranch, CommandLine? provide)
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
            this.provide = provide;
        }

        public void ActivateFunctions(NamespaceInfo currentNamespace)
        {
            if (checkLine != null) checkLine.ActivateFunction(currentNamespace);
            if (doCode != null) doCode.ActivateFunction(currentNamespace);
            if (provide != null) provide.ActivateFunction(currentNamespace);
            foreach (TreeElement treeElement in subBranch) treeElement.ActivateFunctions(currentNamespace);
        }


        public ElementType elementType;
        public List<TreeElement>? subBranch;
        public bool isBranch;
        public CommandLine? checkLine;
        public CommandLine? doCode;
        public CommandLine? provide;


    }
}
