using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SatisfactoryHeaderGenerator
{
    class Program
    {
        private static readonly string[] ClassesToGenerate = new[] {
            "AFGGameState"
        };

        private static readonly Regex FieldRegex = new Regex(@"\/\* (?<comment>.+) \*\/ (?<type>[a-zA-Z]+) ?(?<subtype>[a-zA-Z<>\*,]+)? (?<name>[a-zA-Z\[\]\d]+);", RegexOptions.Singleline | RegexOptions.Compiled);

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine($"Usage: SatisfactoryHeaderGenerator [pdb]");
                return;
            }

            var pdb = args[0];

            foreach (var c in ClassesToGenerate)
            {
                var proc = Process.Start(new ProcessStartInfo()
                {
                    FileName = "pdbex",
                    Arguments = $"{c} {pdb} -j- -k- -p-",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                });

                proc.WaitForExit();

                var line = proc.StandardOutput.ReadLine();
                if (line != $"class {c}")
                {
                    Console.WriteLine($"Unexpected output while parsing {c}: {line}");
                    continue;
                }

                var members = new List<ClassMemberDefinition>();
                while (!proc.StandardOutput.EndOfStream)
                {
                    line = proc.StandardOutput.ReadLine();

                    if (line == "{" || line == "}")
                    {
                        continue;
                    }

                    var match = FieldRegex.Match(line);

                    if (!match.Success)
                    {
                        Console.WriteLine($"Failed match in {c}: {line}");
                        continue;
                    }

                    var def = new ClassMemberDefinition()
                    {
                        Comment = match.Groups["comment"].Value,
                        Type = match.Groups["type"].Value,
                        Subtype = match.Groups["subtype"].Success ? match.Groups["subtype"].Value : null,
                        Name = match.Groups["name"].Value
                    };

                    members.Add(def);
                }
            }
        }
    }
}
