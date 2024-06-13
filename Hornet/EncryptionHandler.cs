using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hornet
{
    internal class EncryptionHandler
    {
        //consructor
        public EncryptionHandler() { }
        public string OneByteXor(string shellcode)
        {
            int key = 0x90;
            string newShellcode = String.Empty;
            for (int i = 1; i < shellcode.Length; i = i + 2)
            {
                string curr_byte = shellcode[i - 1].ToString() + shellcode[i].ToString();
                int hex_byte = Convert.ToInt32(curr_byte, 16);
                int enc_byte = key ^ hex_byte;
                newShellcode = newShellcode + String.Format("{0:x2}", enc_byte);
            }

            return newShellcode;
        }
    }
}
