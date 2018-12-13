using System;
using CommandLine;

namespace UnityOpener
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default
                .ParseArguments<Options>(args)
                .WithParsed(options =>
                    {
                        ExitCode result = new Controller(options).Run(string.IsNullOrEmpty(options.ProjectPath) ? Environment.CurrentDirectory : options.ProjectPath); 
                        if (result != ExitCode.Success)
                        {
                            Console.WriteLine("Failed to open Unity project with error: {0}", result);
                        }
    
                        Environment.Exit((int)result);
                    });
        }
    }
}