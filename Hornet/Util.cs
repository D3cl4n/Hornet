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
        public string outputDir = "C:\\Users\\cdecl\\source\\repos\\Hornet\\Hornet\\payloads";

        //convert the shellcde to c style bytearray
        private string convertShellcode(string shellcode)
        {
            string finalShellcode = String.Empty;
            string prefix = "0x";
            string upperShellcode = shellcode.ToUpper();

            for (int i = 0; i < upperShellcode.Length; i++)
            {
                if ((i + 1) % 2 == 0)
                {
                    finalShellcode = finalShellcode + prefix + upperShellcode[i - 1] + upperShellcode[i] + ",";
                }

                else if ((i + 1) % 20 == 0)
                {
                    finalShellcode = finalShellcode + "\n";
                }
            }

            return finalShellcode;
        }

        //helper method that writes the shellcode to the template code using String.Replace()
        private List<string> writeShellcode(List<string> templateCode, string shellcode)
        {
            EncryptionHandler encryptor = new EncryptionHandler();
            List<string> output = new List<string>();
            string formattedShellcode = encryptor.OneByteXor(shellcode);
            string finalShellcode = convertShellcode(formattedShellcode);
            foreach(string line in templateCode)
            {
                if (line.Contains("payload[] = {}"))
                {
                    string finalStr = "{" + finalShellcode.Remove(finalShellcode.Length - 1) + "}";
                    string newLine = line.Replace("{}", finalStr);
                    output.Add(newLine);
                }
                else 
                {
                    output.Add(line);
                }
            }

            return output;
        }


        //public API function that performs writing to template
        public bool writeToTemplate(string payload, string template)
        {
            bool success = false;
            string templatePath = String.Empty;
            Shellcode shellcode = new Shellcode();
            List<string> newCode = new List<string>();

            if (template.Equals("simple_xor"))
            {
                templatePath = "C:\\Users\\cdecl\\source\\repos\\Hornet\\Hornet\\templates\\simple_xor.c";
                if (!File.Exists(templatePath))
                {
                    Console.WriteLine("[!] Template file does not exist");
                    Environment.Exit(1);
                }
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
                    newCode = writeShellcode(lines, shellcode.calculator);
                }
            }

            using (StreamWriter sw = new StreamWriter(outputDir + "\\payload.c"))
            {
                foreach (string line in newCode)
                {
                    Console.WriteLine(line);
                    sw.WriteLine(line);
                }
            }

            return success;
        }
    }
}
