using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM2.Interpreter
{
    public class VarType
    {
        public bool isNumber, isString, isMalformedString, isBool;

        public static VarType NumberType
        {
            get { return new VarType(true, false, false, false); }
        }
        public static VarType StringType
        {
            get { return new VarType(false, true, false, false); }
        }
        public static VarType MalformedString
        {
            get { return new VarType(false, false, true, false); }
        }
        public static VarType BoolType
        {
            get { return new VarType(false, false, false, true); }
        }

        public VarType(bool n, bool s, bool ms, bool b)
        {
            isNumber = n;
            isString = s;
            isMalformedString = ms;
            isBool = b;
        }

        public static bool operator ==(VarType v1, VarType v2)
        {
            bool isN = v1.isNumber == v2.isNumber;
            bool isS = v1.isString == v2.isString;
            bool isMS = v1.isMalformedString == v2.isMalformedString;
            bool isB = v1.isBool == v2.isBool;
            return isN && isS && isMS && isB;
        }
        public static bool operator !=(VarType v1, VarType v2)
        {
            return !(v1 == v2);
        }
        public override bool Equals(object obj)
        {
            if(obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            return this == (VarType)obj;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public class TypeParser
    {
        public static VarType ParseValue(string var)
        {
            bool isNumber=false, isString = false, isMalformedString = false, isBool = false;

            isNumber = IntUtils.FromString(var) != null;
            isString = var[0] == '"' && var[var.Length - 1] == '"';
            isMalformedString = (var[0] == '"' || var[var.Length - 1] == '"') && (var[0] != var[var.Length - 1]);
            isBool = var == "true" || var == "false";

            return new VarType(isNumber, isString, isMalformedString, isBool);
        }
    }
}
