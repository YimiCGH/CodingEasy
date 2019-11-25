using UnityEngine;
using System.Collections;
using System.Text;

namespace YimiTools.EncryptUtil
{
    //混淆
    public class Obfuscate
    {
        public static string ObfuscateString(string _Original,string _Key) {
            byte[] bytes = Encoding.ASCII.GetBytes(_Original);
            byte[] buffer2 = Encoding.ASCII.GetBytes(_Key);
            int lenght = buffer2.Length;
            byte[] buffer3 = new byte[bytes.Length];

            for (int i = 0; i < buffer3.Length; i++)
            {
                buffer3[i] = (byte)(bytes[i] ^ buffer2[i % lenght]);
            }
            return Encoding.ASCII.GetString(buffer3);
        }
    }
}
