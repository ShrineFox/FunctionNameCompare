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
        public static List<Tuple<string, string>> oldFunctions = new List<Tuple<string, string>>();
        public static List<Tuple<string, string>> newFunctions = new List<Tuple<string, string>>();

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

            //Get function names from old and new libraries
            GetFunctionNames(Options.Old, Options.New);

            //If you're editing a flowscript rather than updating a library...
            if (!Options.Update)
            {
                //If the input flowscript exists...
                if (File.Exists(Options.Input))
                {
                    //Replace each function name in .flow where names don't match
                    string flowscript = Options.Input;
                    var flowText = File.ReadAllLines(flowscript);
                    var newFlowText = new List<string>();

                    //Foreach line in the flowscript...
                    for (int i = 0; i < flowText.Length; i++)
                    {
                        //foreach old function...
                        for (int x = 0; x < oldFunctions.Count(); x++)
                        {
                            //If flowscript line contains old function...
                            if (flowText[i].Contains(oldFunctions[x].Item2))
                            {
                                Tuple<string, string> newFunction = newFunctions.First(f => f.Item1.Equals(oldFunctions[x].Item1));
                                if (Options.Print)
                                    Console.WriteLine($"{oldFunctions[x].Item2} ==> {newFunction.Item2}");
                                newFlowText.Add(flowText[i].Replace(oldFunctions[x].Item2, newFunction.Item2));
                            }
                            else
                            {
                                newFlowText.Add(flowText[i]);
                            }
                        }
                    }
                    //Overwrite flowscript with function names updated
                    File.WriteAllText(flowscript, String.Join("\n", flowText));
                }
                else
                {
                    Console.WriteLine("Could not find input flowscript file. Aborting...");
                    return;
                }
            }
            else
            {
                // Replace the name of each function in the old library with one with a matching ID from the new library
                foreach (var functionJson in Directory.GetFiles(Options.Old, "*Functions.json", SearchOption.AllDirectories))
                {
                    var lines = File.ReadAllLines(functionJson);
                    List<string> newLines = new List<string>();
                    //Foreach line in old library function json...
                    for (int i = 0; i < lines.Length; i++)
                    {
                        //Foreach function json line that contains an old function ID...
                        if (oldFunctions.Any(o => lines[i].Contains(o.Item1)))
                        {
                            string index = lines[i];
                            string returnType = lines[i + 1];
                            string oldName = lines[i + 2].Replace("    \"Name\": \"", "").Replace("\",", "");
                            string newName = newFunctions.Single(f => f.Item1.Equals(index.Replace("    \"Index\": \"", "").Replace("\",", ""))).Item2.Replace("    \"Name\": \"", "").Replace("\",", "");

                            if (Options.Print)
                                Console.WriteLine($"{oldName} <== {newName}");
                            newLines.Add(index);
                            newLines.Add(returnType);
                            newLines.Add(lines[i + 2].Replace(oldName, newName));
                            i = i + 2;
                        }
                        else
                        {
                            newLines.Add(lines[i]);
                        }  
                    }
                    //Overwrite function json with function names updated
                    File.WriteAllText(functionJson, String.Join("\n", newLines));
                }

            }
            Console.WriteLine("Done");
            Console.ReadKey();
        }

        private static void GetFunctionNames(string oldLibrary, string newLibrary)
        {

            foreach (var functionJson in Directory.GetFiles(oldLibrary, "*Functions.json", SearchOption.AllDirectories))
            {
                var lines = File.ReadAllLines(functionJson);
                for (int i = 0; i < lines.Length; i++)
                    if (lines[i].StartsWith("    \"Index\": \""))
                    {
                        oldFunctions.Add(new Tuple<string, string>(lines[i].Replace("    \"Index\": \"", "").Replace("\",", ""), lines[i + 2].Replace("    \"Name\": \"", "").Replace("\",", "")));
                        if (Options.Print)
                            Console.WriteLine($"Old Function:\n\tIndex: {oldFunctions.Last().Item1}\n\tName: {oldFunctions.Last().Item2}");
                    }
            }

            foreach (var functionJson in Directory.GetFiles(newLibrary, "*Functions.json", SearchOption.AllDirectories))
            {
                var lines = File.ReadAllLines(functionJson);
                for (int i = 0; i < lines.Length; i++)
                    if (lines[i].StartsWith("    \"Index\": \""))
                    {
                        newFunctions.Add(new Tuple<string, string>(lines[i].Replace("    \"Index\": \"", "").Replace("\",", ""), lines[i + 2].Replace("    \"Name\": \"", "").Replace("\",", "")));
                        if (Options.Print)
                            Console.WriteLine($"New Function:\n\tIndex: {oldFunctions.Last().Item1}\n\tName: {oldFunctions.Last().Item2}");
                    }
            }
        }
    }

    public class ProgramOptions
    {
        [Option("o", "old", "directory", "The path to the old AtlusScriptCompiler library directory.", Required = true)]
        public string Old { get; set; } = "";

        [Option("n", "new", "directory", "The path to the new AtlusScriptCompiler library directory (the one you want to use function names of).", Required = true)]
        public string New { get; set; } = "";

        [Option("i", "input", "flowscript", "The path to the flowscript to modify.")]
        public string Input { get; set; } = "";

        [Option("p", "print", "boolean", "When true, prints all differences between libraries.")]
        public bool Print { get; set; } = false;

        [Option("u", "update", "boolean", "When true, updates old library with new function names.")]
        public bool Update { get; set; } = false;
    }
}
