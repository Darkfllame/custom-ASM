using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM2.Interpreter
{
    public class InitKeywords : Initializer
    {
        public void Init()
        {
            Interpreter.AddKeyword("set", SetKeyword);
            Interpreter.AddKeyword("free", FreeKeyword);
            Interpreter.AddKeyword("get", GetKeyword);

            Interpreter.AddKeyword("out", OutKeyword);

            Interpreter.AddKeyword("func", FuncKeyword);
            Interpreter.AddKeyword("call", FuncKeyword);
        }

        public void SetKeyword(int l, string[] lines, string code, string[] args)
        {
            if (args.Length != 2)
                throw new Exception("Need only/at least 2 arguments");

            string arg1 = args[0];
            string arg2 = args[1];

            if (Interpreter.variables.ContainsKey(arg1) && Interpreter.variables.ContainsKey(arg2))
            {
                Interpreter.variables[arg1].Value = Interpreter.variables[arg2].Value;
            }else if(Interpreter.variables.ContainsKey(arg1) && !Interpreter.variables.ContainsKey(arg2))
            {
                Variable val = new Variable(arg2);
                Interpreter.variables[arg1] = val;
            }else if(TypeParser.ParseValue(arg1) == VarType.NumberType && Interpreter.variables.ContainsKey(arg2))
            {
                Interpreter.memory[Math.Abs((int)IntUtils.FromString(arg1))] = Interpreter.variables[arg2];
            }else if(TypeParser.ParseValue(arg1) == VarType.NumberType && !Interpreter.variables.ContainsKey(arg2))
            {
                Variable val = new Variable(arg2);
                Interpreter.memory[Math.Abs((int)IntUtils.FromString(arg1))] = val;
            }
            else throw new Exception("Can't parse type : " + l);
        }
        public void FreeKeyword(int l, string[] lines, string code, string[] args)
        {
            if (args.Length != 1)
                throw new Exception("Need only/at least 1 arguments");

            string arg = args[0];

            if (Interpreter.variables.ContainsKey(arg))
            {
                Interpreter.variables.Remove(arg);
                GC.Collect();
            }
            else if (TypeParser.ParseValue(arg) == VarType.NumberType)
            {
                Interpreter.memory.RemoveAt(Math.Abs((int)IntUtils.FromString(arg)));
                GC.Collect();
            }
            else throw new Exception("Can't parse type : "+l);
        }
        public void GetKeyword(int l, string[] lines, string code, string[] args)
        {
            if (args.Length != 2)
                throw new Exception("Need only/at least 2 arguments");

            string arg1 = args[0];
            string arg2 = args[1];
            
            if (TypeParser.ParseValue(arg1) == VarType.NumberType && Interpreter.variables.ContainsKey(arg2))
            {
                Interpreter.variables[arg2].Value = Interpreter.memory[Math.Abs((int)IntUtils.FromString(arg1))].Value;
            }
            else throw new Exception("Can't parse type : " + l);
        }

        public void OutKeyword(int l, string[] lines, string code, string[] args)
        {
            if (args.Length != 1)
                throw new Exception("Need only/at least 1 arguments");

            string arg = args[0];
            VarType argT = TypeParser.ParseValue(arg);

            if (Interpreter.variables.ContainsKey(arg))
            {
                Variable var = Interpreter.variables[arg];
                if(var.IsVariable())
                    Console.WriteLine(var.Value);
            }else if (argT == VarType.NumberType || argT == VarType.BoolType)
            {
                Console.WriteLine(arg);
            }else if (argT == VarType.StringType)
            {
                string toOut = StringUtils.Separate(arg, '"')[0];
                Console.WriteLine(toOut);
            }else if (argT == VarType.MalformedString)
            {
                throw new Exception("got malformed string at line : " + l);
            }else throw new Exception("Can't parse argument : " + l);
        }

        //TODO: redo all the func and call keyword with more recent api ;(
        public void FuncKeyword(int l, string[] lines, string code, string[] args)
        {
            if (!(args.Length != 1))
                throw new Exception("need only/at least 1 argument");

            string arg = args[0];

            bool isString = (arg[0] == '"' && arg[arg.Length - 1] == '"');
            bool malformedString = (arg[0] == '"' || arg[arg.Length - 1] == '"') && (arg[0] != arg[arg.Length - 1]);
            bool isNumber = IntUtils.FromString(arg) != null;
            bool isVar = Interpreter.variables.ContainsKey(arg);
            if (isString) throw new Exception("can't set a string as function name");
            if (malformedString) throw new Exception("malformed string at line " + l);
            if (isNumber) throw new Exception("can't set a number as function name");

            int e = 0;
            int j = l;
            for (; j < lines.Length; j++)
            {
                string[] lines1 = StringUtils.Separate(code, '\n').ToArray();
                string word1 = (lines1.Length >= 1) ? lines1[0] : "";

                if (word1 == "endfunc")
                {
                    e = j;
                    break;
                }
            }
            if (e == 0 && j + 2 > lines.Length)
            {
                throw new Exception("endfunc not found : " + l);
            }

            if (isVar)
            {
                if (Interpreter.variables[arg].IsFunction())
                {
                    Interpreter.variables[arg] = new Variable(l, e);
                    GC.Collect();
                }
                else throw new Exception("can't overwrite non-function variable");
            }
            else
            {
                Interpreter.variables.Add(arg, new Variable(l, e));
            }
        }
        public void CallKeyword(int l, string[] lines, string code, string[] args)
        {
            if (!(args.Length != 1))
                throw new Exception("need only/at least 1 argument");

            string arg = args[0];

            bool isString = (arg[0] == '"' && arg[arg.Length - 1] == '"');
            bool malformedString = (arg[0] == '"' || arg[arg.Length - 1] == '"') && (arg[0] != arg[arg.Length - 1]);
            bool isNumber = IntUtils.FromString(arg) != null;
            bool isVar = Interpreter.variables.ContainsKey(arg);
            if (isString) throw new Exception("can't set a string as function name");
            if (malformedString) throw new Exception("malformed string at line " + l);
            if (isNumber) throw new Exception("can't set a number as function name");
            if (isVar)
            {
                Interpreter.variables[arg].Call(code);
            }
            else throw new Exception("can't find function '" + arg + "'");
        }
    }
}
