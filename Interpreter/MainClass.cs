using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM2.Interpreter
{
    public class MainClass
    {
        public static readonly string consoleDir = Environment.CurrentDirectory+"\\";
        public static readonly string fileExt = ".csm";
        public static readonly char folderSep = (Environment.OSVersion.Platform == PlatformID.Unix)? '/' : '\\';

        public static void Main(string[] args)
        {
            Console.Title = "ASM2 interpreter";
            Interpreter.toInit.Add(new InitKeywords());
            Interpreter.Init();

            string fileName = "";
            if(0 < args.Length)
            {
                if (File.Exists(args[0]))
                    fileName = args[0];
                else
                    fileName = consoleDir+args[0];

                Interpreter.OpenFile(fileName);
            }

            Mainloop(fileName);
        }
        
        public static string findFileExt(string _fileName)
        {
            if (_fileName[_fileName.Length - 1] == folderSep)
                throw new IOException("given value is not a file, it's a folder");
            string[] infos = StringUtils.Separate(_fileName, '.').ToArray();
            string ext = "."+ infos[infos.Length - 1];
            return ext;
        }

        public static void Mainloop(string _fileName)
        {
            while (true)
            {
                Console.Write(consoleDir + ">");
                string input = Console.ReadLine();

                string[] words = StringUtils.Separate(input, ' ').ToArray();

                if (words.Length > 0)
                {
                    switch (words[0])
                    {
                        case "exit":
                            Environment.Exit(0);
                            break;
                        case "open":
                            if (words.Length < 2 || words.Length > 2)
                            {
                                Console.WriteLine("Need only 1 arguments");
                                break;
                            }

                            if (File.Exists(words[1]))
                                _fileName = words[1];
                            else
                                _fileName = consoleDir + words[1];

                            Interpreter.OpenFile(_fileName);

                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
