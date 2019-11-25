using UnityEngine;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using System;
using System.IO;

namespace YimiTools.EncryptUtil
{
    public class RijndaelEncrypt
    {
        // Fields
        public static string IV;
        public static string Key;

        // Methods
        public RijndaelEncrypt() {
            Key = "s*#_das0va?d@`%6q&+|qe45ca(yimi)";
            IV = "jasd+dxs(ock#%0)";
        }
        public RijndaelEncrypt(string _IV, string _Key) {
            IV = _IV;
            Key = _Key;
        }
        public static string Decrypt(byte[] _Encrypted) {

            string str = "";
            byte[] bytes = Encoding.ASCII.GetBytes(Key);
            byte[] buffer2 = Encoding.ASCII.GetBytes(IV);
            try
            {
                using (RijndaelManaged managed = new RijndaelManaged())
                {
                    managed.Key = bytes;
                    managed.IV = buffer2;
                    str = DecryptStringFromBytes(_Encrypted, managed.Key, managed.IV);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;

        }
        private static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV) {

            if ((cipherText == null) || (cipherText.Length == 0))
            {
                throw new ArgumentNullException("cipherText");
            }
            if ((Key == null) || (Key.Length == 0))
            {
                throw new ArgumentNullException("Key");
            }
            if ((IV == null) || (IV.Length == 0))
            {
                throw new ArgumentNullException("IV");
            }
            using (RijndaelManaged managed = new RijndaelManaged())
            {
                managed.Key = Key;
                managed.IV = IV;
                ICryptoTransform transform = managed.CreateDecryptor(managed.Key, managed.IV);
                using (MemoryStream stream = new MemoryStream(cipherText))
                {
                    using (CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(stream2))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }

        }
        public static byte[] Encrypt(string _Original) {
            byte[] buffer = null;
            byte[] bytes = Encoding.ASCII.GetBytes(Key);
            byte[] buffer3 = Encoding.ASCII.GetBytes(IV);
            try
            {
                using (RijndaelManaged managed = new RijndaelManaged())
                {
                    managed.Key = bytes;
                    managed.IV = buffer3;
                    buffer = EncryptStringToBytes(_Original, managed.Key, managed.IV);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return buffer;

        }
        private static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV) {
            if ((plainText == null) || (plainText.Length <= 0))
            {
                throw new ArgumentNullException("plainText");
            }
            if ((Key == null) || (Key.Length == 0))
            {
                throw new ArgumentNullException("Key");
            }
            if ((IV == null) || (IV.Length == 0))
            {
                throw new ArgumentNullException("IV");
            }
            using (RijndaelManaged managed = new RijndaelManaged())
            {
                managed.Key = Key;
                managed.IV = IV;
                ICryptoTransform transform = managed.CreateEncryptor(managed.Key, managed.IV);
                using (MemoryStream stream = new MemoryStream())
                {
                    using (CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(stream2))
                        {
                            writer.Write(plainText);
                        }
                        return stream.ToArray();
                    }
                }
            }

        }
    }
}
