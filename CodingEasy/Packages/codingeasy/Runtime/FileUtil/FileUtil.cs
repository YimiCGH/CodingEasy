using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

namespace YimiTools.FileUtils {
    public class FileUtil
    {


        #region 读取文件
        /// <summary>
        /// 读取UTF-8 文件时，注意，带签名和不带签名是有区别的，特别是我们做逐行读取时，注意排除签名的干扰
        /// </summary>
        /// <param name="_FullPath"></param>
        /// <returns></returns>

        public string GetFileContent(string _FullPath)
        {
            if (File.Exists(_FullPath))
            {
                using (StreamReader reader = new StreamReader(File.OpenRead(_FullPath), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            else
            {
                Debug.LogError("File.Exists = false:" + _FullPath);
            }
            return null;
        }
        public byte[] GetFileContent_ToBytes(string _FullPath)
        {
            byte[] buffer = null;
            if (File.Exists(_FullPath))
            {
                return File.ReadAllBytes(_FullPath);
            }
            Debug.LogError("File.Exists = false");
            return buffer;
        }
        /*
        public static string GetFileContex_StreamingAsset(string _FullFilePath)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    _FullFilePath = string.Format("file:///{0}", _FullFilePath);
                    break;

                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                    break;

                default:
                    Debug.LogError("未定义平台");
                    break;
            }

            //TODO 改为 UnityWebRequest
            using (UnityWebRequest uwr = UnityWebRequest.Get(_FullFilePath)) {
                yield return webRequest.SendWebRequest();
            }

            using (WWW www = new WWW(_FullFilePath))
            {
                while (!www.isDone)
                {
                    if (!string.IsNullOrEmpty(www.error))
                    {
                        Debug.LogError(www.error);
                        return null;
                    }
                }
                return www.text;
            }

        }
        */
        #endregion

        #region 写入文件

        public static void WriteFileContent(string _FullPath, byte[] _Contents)
        {
            if (!File.Exists(_FullPath))
            {
                CreateFile(_FullPath, _Contents);
            }
            else
            {
                File.WriteAllBytes(_FullPath, _Contents);
            }
        }
        public static void WriteFileContent(string _FullPath, string _Contents, string encodingName = "utf-8")
        {
            if (!File.Exists(_FullPath))
            {
                CreateFile(_FullPath, Encoding.GetEncoding(encodingName).GetBytes(_Contents));
            }
            else
            {
                File.WriteAllText(_FullPath, _Contents, Encoding.GetEncoding(encodingName));
            }
        }
        #endregion

        #region 创建文件
        private static void CreateFile(string _FullPath, byte[] _Data)
        {
            DirectoryInfo info = new DirectoryInfo(_FullPath);
            if (!info.Exists)
            {
                info.Parent.Create();
            }
            using (FileStream stream = File.Create(_FullPath))
            {
                stream.Write(_Data, 0, _Data.Length);
            }
        }
        #endregion

        #region 读取XML
        public static XmlDocument GetXMLDoc(string _FullFilePath)
        {
            if ((Application.platform == RuntimePlatform.WindowsPlayer) || (Application.platform ==  RuntimePlatform.WindowsEditor))
            {
                if (File.Exists(_FullFilePath))
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(_FullFilePath);
                    return document;
                }
            }
            //TODO 安卓平台实现
            //else
            //{
            //    string str = GetFileContex_StreamingAsset(_FullFilePath);
            //    if (!string.IsNullOrEmpty(str))
            //    {
            //        XmlReader reader = XmlReader.Create(new StringReader(str));
            //        XmlDocument document3 = new XmlDocument();
            //        document3.Load(reader);
            //        return document3;
            //    }
            //}
            Debug.LogError("找不到文件 :" + _FullFilePath);
            return null;
        }
        #endregion


        #region 文件编码类型
        public static Encoding GetEncodingType(string _Path)
        {
            using (FileStream stream = new FileStream(_Path, FileMode.Open, FileAccess.Read))
            {
                return GetType(stream);
            }
        }
        public static Encoding GetType(FileStream fs)
        {
            BinaryReader reader = new BinaryReader(fs, Encoding.Default);
            byte[] buffer = reader.ReadBytes(3);
            reader.Close();
            if ((buffer == null) || (buffer.Length == 0))
            {
                object[] objArray1 = new object[] { fs.Name };
                Debug.LogWarningFormat("{0} == null 或 length = 0 ", objArray1);
                return Encoding.Default;
            }
            if (buffer[0] >= 0xef)
            {
                if (((buffer[0] == 0xef) && (buffer[1] == 0xbb)) && (buffer[2] == 0xbf))
                {
                    return Encoding.UTF8;
                }
                if ((buffer[0] == 0xfe) && (buffer[1] == 0xff))
                {
                    return Encoding.BigEndianUnicode;
                }
                if ((buffer[0] == 0xff) && (buffer[1] == 0xfe))
                {
                    return Encoding.Unicode;
                }
                return Encoding.Default;
            }
            return Encoding.Default;
        }
        #endregion

    }
}

