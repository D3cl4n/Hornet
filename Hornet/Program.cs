using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Hornet
{
    internal class Program
    {
        public class Options
        {
            [Option(Required = false, HelpText = "template to use")]
            public string? Payload { get; set; }
            [Option(Required = false, HelpText = "list available payloads")]
            public bool List { get; set; }
        }

        static void RunOptions(Options opts)
        {
            Util util = new Util();
            Console.WriteLine("[+] CLI argument parsing successful");
            if (opts.List)
            {
                Console.WriteLine("[+] Available payloads:\n");
                Console.WriteLine("\t[1] calc");
            }

            else if (opts.Payload != null)
            {
                Console.WriteLine("[+] Generating payload binary from template");
                if (!util.payloadList.Contains(opts.Payload)) 
                {
                    Console.Error.WriteLine("[!] Invalid payload option");
                    Environment.Exit(1);
                }

                
            }
        }

        static void HandleParseErrors(IEnumerable<Error> errors)
        {
            Environment.Exit(0);
        }
        static void Main(string[] args)
        {
            //handle CLI args
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions).WithNotParsed(HandleParseErrors);
        }
    }
}
