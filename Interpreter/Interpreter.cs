using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM2.Interpreter
{
    public class Interpreter
    {
        public static Dictionary<string, Variable> variables = new Dictionary<string, Variable>();
        public static Dictionary<string, Action<int, string[], string, string[]>> keywords = new Dictionary<string, Action<int, string[], string, string[]>>();
        public static List<Variable> memory = new List<Variable>();

        public static List<Initializer> toInit = new List<Initializer>();
        private static bool init_d = false;
        public static void Init()
        {
            init_d = true;
            foreach(var i in toInit)
            {
                i.Init();
            }
        }
        public static void OpenFile(string _fileName)
        {
            if (!File.Exists(_fileName))
            {
                Console.WriteLine("File Not Found");
                return;
            }
            string text = File.ReadAllText(_fileName);
            Run(text);
        }
        public static void Run(string code, bool isInFunc = false, int startl = 0, int endl = 0)
        {
            if (!init_d) throw new Exception("Interpreter is not initialized");
            string[] lines = StringUtils.Separate(code, '\n').ToArray();

            for (int l = (isInFunc)?startl:0; l < ((isInFunc)? Mathf.Clamp(endl, 0, lines.Length): lines.Length); l++)
            {
                string line = lines[l];
                string[] words = StringUtils.Separate(line, ' ').ToArray();
                List<string> t = new List<string>(words);
                t.RemoveAt(0);
                string[] args = t.ToArray();

                if(words.Length > 0)
                {
                    string keyword = words[0];
                    if (keywords.ContainsKey(keyword))
                    {
                        keywords[keyword].Invoke(l, lines, code, args);
                    }
                }
            }
        }

        public static void AddKeyword(string name, Action<int, string[], string, string[]> f)
        {
            keywords.Add(name, f);
        }
    }
}