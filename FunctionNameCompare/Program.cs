using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGE.SimpleCommandLine;

namespace FunctionNameCompare
{
    class Program
    {
        public static ProgramOptions Options { get; private set; }

        static void Main(string[] args)
        {
            //Validate input
            string about = SimpleCommandLineFormatter.Default.FormatAbout<ProgramOptions>("ShrineFox, TGE", "Rename Functions in a .flow document by comparing one AtlusScriptCompiler Library to another.");
            Console.WriteLine(about);
            try
            {
                Options = SimpleCommandLineParser.Default.Parse<ProgramOptions>(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            string flowscript = Options.Input;

            //Replace each function name in .flow where names don't match
            List<Tuple<string, string>> functionNames = GetFunctionNames(Options.Old, Options.New);
            string flowText = File.ReadAllText(flowscript);
            foreach (var function in functionNames)
            {
                if (function.Item1 != function.Item2)
                {
                    flowText = flowText.Replace(function.Item1, function.Item2);
                    Console.WriteLine($"Replacing {function.Item1} with {function.Item2}");
                }
            }
                
            File.WriteAllText(flowscript, flowText);
        }

        private static List<Tuple<string, string>> GetFunctionNames(string oldLibrary, string newLibrary)
        {
            List<Tuple<string, string>> functionNames = new List<Tuple<string, string>>();
            List<string> newFunctions = new List<string>();
            List<string> oldFunctions = new List<string>();

            foreach (var functionJson in Directory.GetFiles(oldLibrary, "*Functions.json", SearchOption.AllDirectories))
            {
                var lines = File.ReadAllLines(functionJson);
                for (int i = 0; i < lines.Length; i++)
                    if (lines[i].StartsWith("    \"Name\": \""))
                        oldFunctions.Add(lines[i].Replace("    \"Name\": \"", "").Replace("\",", ""));
            }

            foreach (var functionJson in Directory.GetFiles(newLibrary, "*Functions.json", SearchOption.AllDirectories))
            {
                var lines = File.ReadAllLines(functionJson);
                for (int i = 0; i < lines.Length; i++)
                    if (lines[i].StartsWith("    \"Name\": \""))
                        newFunctions.Add(lines[i].Replace("    \"Name\": \"", "").Replace("\",", ""));
            }

            for (int i = 0; i < oldFunctions.Count(); i++)
                functionNames.Add(new Tuple<string, string>(oldFunctions[i], newFunctions[i]));

            return functionNames;
        }
    }

    public class ProgramOptions
    {
        [Option("o", "old", "directory", "The path to the old AtlusScriptCompiler library directory.", Required = true)]
        public string Old { get; set; } = "";

        [Option("n", "new", "directory", "The path to the new AtlusScriptCompiler library directory (the one you want to use function names of).", Required = true)]
        public string New { get; set; } = "";

        [Option("i", "input", "flowscript", "The path to the flowscript to modify.", Required = true)]
        public string Input { get; set; } = "";
    }
}
