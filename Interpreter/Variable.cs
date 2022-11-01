using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM2.Interpreter
{
    public class Variable
    {
        public VarType type
        {
            get { return (IsVariable())?TypeParser.ParseValue(Value):new VarType(false, false, false, false); }
        }
        public string Value
        {
            get;
            set;
        }

        public int FuncIndex
        {
            get;
            set;
        }
        private readonly bool IsFunc = false;
        public int FuncEndIndex
        {
            get;
            set;
        }

        public Variable(string val)
        {
            Value = val;
            FuncIndex = 0;
            FuncEndIndex = 0;
            IsFunc = false;
        }
        public Variable(int func, int endI)
        {
            Value = "null";
            FuncIndex = func;
            FuncEndIndex = endI;
            IsFunc = true;
        }

        public void Call(string _code)
        {
            if (IsFunc)
            {
                Interpreter.Run(_code, true, FuncIndex + 1, FuncEndIndex);
            }
        }

        public bool IsVariable()
        {
            return Value != null && !IsFunc;
        }
        public bool IsFunction()
        {
            return IsFunc;
        }
        public bool IsNull()
        {
            return (Value == "null" || Value == null) && IsFunc == false;
        }

        public int ToNumber()
        {
            if (!IsFunc && Value != null)
            {
                return (int)IntUtils.FromString(Value);
            }
            return 0;
        }
        public bool ToBoolean()
        {
            if(!IsFunc && Value != null)
            {
                return (Value.ToLower() == "true") ? true : false;
            }
            return false;
        }
    }
}
