﻿namespace TASI.RuntimeObjects.VarClasses
{
    public class VarConstruct
    {
        public enum VarType
        {
            num, @string, @bool, @void, @int, list, all
        }
        public string name;
        public VarType type;
        public bool isLink;
        public bool isConstant;

        public VarConstruct(VarType type, string name, bool isLink = false, bool isConst = false)
        {
            this.name = name.ToLower();
            this.type = type;
            this.isLink = isLink;
            isConstant = isConst;
        }
    }
}