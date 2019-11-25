using UnityEngine;
using System.Collections;
using System;
using System.Security.Cryptography;
using System.Text;

namespace YimiTools.EncryptUtil
{
    public class TripleDESEncrypt 
    {
        private const string hash = "s!a?d*%3$d5@8+";

        public static string Decrypt(string _Origin, string _Key = hash)
        {
            string str;
            byte[] inputBuffer = Convert.FromBase64String(_Origin);
            using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
            {
                byte[] buffer2 = provider.ComputeHash(Encoding.UTF8.GetBytes(_Key));
                TripleDESCryptoServiceProvider provider1 = new TripleDESCryptoServiceProvider
                {
                    Key = buffer2,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                using (TripleDESCryptoServiceProvider provider2 = provider1)
                {
                    byte[] bytes = provider2.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
                    str = Encoding.UTF8.GetString(bytes);
                }
            }
            return str;

        }
        public static string Encrypt(string _Origin, string _Key = hash) {
            string str;
            byte[] bytes = Encoding.UTF8.GetBytes(_Origin);
            using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
            {
                byte[] buffer2 = provider.ComputeHash(Encoding.UTF8.GetBytes(_Key));
                TripleDESCryptoServiceProvider provider1 = new TripleDESCryptoServiceProvider
                {
                    Key = buffer2,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                using (TripleDESCryptoServiceProvider provider2 = provider1)
                {
                    byte[] inArray = provider2.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
                    str = Convert.ToBase64String(inArray, 0, inArray.Length);
                }
            }
            return str;

        }
    }
}