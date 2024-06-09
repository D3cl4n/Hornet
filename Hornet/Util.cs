using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Hornet
{
    /*
     * Util: class to handle file IO and writing to the templates
     * payloadList: list of the supported payloads that I have shellcode for
     */
    internal class Util
    {
        //constructor
        public Util() { }
        public List<string> payloadList = new List<string> { "calc" };

        //takes the shellcode string as ascii and formats to C bytearray
        private string FormatShellcode(string shellcode)
        {
            string newShellcode = String.Empty;

            return newShellcode;
        }

        //helper method that writes the shellcode to the template code using String.Replace()
        private List<string> writeShellcode(List<string> templateCode, string shellcode)
        {
            List<string> output = new List<string>();
            string formattedShellcode = FormatShellcode(shellcode);
            foreach(string line in templateCode)
            {
                if (line.Contains("payload[] = {}"))
                {
                    line.Replace("{}", formattedShellcode);
                }
                output.Add(line);
            }

            return output;
        }

        //public API function that performs writing to template
        public bool writeToTemplate(string payload)
        {
            bool success = false;
            Shellcode shellcode = new Shellcode();
            string templatePath = Directory.GetCurrentDirectory() + "\\template.txt"; //TODO: change this since exe runs form bin/

            if (!File.Exists(templatePath))
            {
                Console.Error.WriteLine("[!] Template file does not exist");
                Environment.Exit(1);
            }

            using (StreamReader sr = File.OpenText(templatePath))
            {
                string s;
                List<string> lines = new List<string>();

                while ((s = sr.ReadLine()) != null)
                {
                    lines.Add(s);
                }

                if (payload.Equals("calc"))
                {
                    List<string> newCode = writeShellcode(lines, shellcode.calculator);
                }
            }

            return success;
        }
    }
}
