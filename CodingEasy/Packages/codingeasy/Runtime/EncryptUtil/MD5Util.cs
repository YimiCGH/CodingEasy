using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;
using System.Text;
using System.IO;
namespace YimiTools.EncryptUtil {
    public class MD5Util
    {
        public static string CalculateMD5(string _data)
        {
            string str;
            str = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(_data))).Replace("-", "").ToLower();
            return str;
        }

        public static string CalculateMD5(byte[] _data)
        {
            string str;
            MD5 md = MD5.Create();
            string s = BitConverter.ToString(_data);
            str = BitConverter.ToString(md.ComputeHash(Encoding.UTF8.GetBytes(s))).Replace("-", "").ToLower();
            return str;
        }

        public static string GetFileMD5(string _filePath)
        {
            string str = string.Empty;
            if (File.Exists(_filePath))
            {
                string s = File.ReadAllText(_filePath);
                str = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(s))).Replace("-", "").ToLower();

            }
            return str;
        }
    }

}
