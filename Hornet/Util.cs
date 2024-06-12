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

        //takes the shellcode string as ascii and formats to C bytearray
        private string FormatShellcode(string shellcode)
        {
            int key = 0x90;
            string newShellcode = String.Empty;
            for (int i = 1; i < shellcode.Length; i = i + 2)
            {
                string curr_byte = shellcode[i-1].ToString() + shellcode[i].ToString();
                int hex_byte = Convert.ToInt32(curr_byte, 16);
                int enc_byte = key ^ hex_byte;
                newShellcode = newShellcode + String.Format("{0:x2}", enc_byte);
            }


            return newShellcode;
        }

        //helper method that writes the shellcode to the template code using String.Replace()
        private List<string> writeShellcode(List<string> templateCode, string shellcode)
        {
            List<string> output = new List<string>();
            string formattedShellcode = FormatShellcode(shellcode);
            string finalShellcode = convertShellcode(formattedShellcode);
            foreach(string line in templateCode)
            {
                if (line.Contains("payload[] = {}"))
                {
                    Console.WriteLine("HEREHEREHERE");
                    Console.WriteLine(finalShellcode);
                    string newLine = line.Replace("{}", finalShellcode);
                    Console.WriteLine(newLine);
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
        public bool writeToTemplate(string payload)
        {
            bool success = false;
            Shellcode shellcode = new Shellcode();
            List<string> newCode = new List<string>();
            string templatePath = "C:\\Users\\cdecl\\source\\repos\\Hornet\\Hornet\\template.txt"; //TODO: change this since exe runs form bin/

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
                    newCode = writeShellcode(lines, shellcode.calculator);
                }
            }

            using (StreamWriter sw = new StreamWriter(templatePath))
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
