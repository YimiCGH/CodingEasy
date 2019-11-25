using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace YimiTools.LogUtil {
    public static class DebugEx
    {
        const string black = "000000FF";
        public static void Log<T>(ICollection<T> _Objs, string _Color = black, string _Tag = "RTLog", bool _IsBold = false, float _Size = 12f)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("count = " + _Objs.Count);
            for (int i = 0; i < _Objs.Count; i++)
            {
                builder.AppendLine(string.Format("<b>{0}</b>:{1}", i, _Objs.ElementAt<T>(i).ToString()));
            }
            Log(builder.ToString(), _Color, _Tag, _IsBold, _Size);
        }
        public static void Log(string _Content = "", string _Color = black, string _Tag = "RTLog", bool _IsBold = false, float _Size = 12f)
        {
            if (_IsBold)
            {
                object[] args = new object[] { _Size, _Color, _Content, _Tag };
                Debug.Log(string.Format("<b>[{3}]:</b> <b><size={0}><color=\"#{1}\">{2}</color></size></b>", args));
            }
            else
            {
                object[] objArray2 = new object[] { _Size, _Color, _Content, _Tag };
                Debug.Log(string.Format("<b>[{3}]:</b> <size={0}><color=\"#{1}\">{2}</color></size>", objArray2));
            }
        }


        public static void LogError<T>(ICollection<T> _Objs, string _Color = black, string _Tag = "RTLog", bool _IsBold = false, float _Size = 12f)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < _Objs.Count; i++)
            {
                builder.AppendLine(_Objs.ElementAt(i).ToString());
            }
            LogError(builder.ToString(), _Color, _Tag, _IsBold, _Size);
        }


        public static void LogError(string _Content = "", string _Color = black, string _Tag = "RTLog", bool _IsBold = false, float _Size = 12f)
        {
            if (_IsBold)
            {
                object[] args = new object[] { _Size, _Color, _Content, _Tag };
                Debug.LogError(string.Format("<b>[{3}]:</b> <b><size={0}><color=\"#{1}\">{2}</color></size></b>", args));
            }
            else
            {
                object[] objArray2 = new object[] { _Size, _Color, _Content, _Tag };
                Debug.LogError(string.Format("<b>[{3}]:</b> <size={0}><color=\"#{1}\">{2}</color></size>", objArray2));
            }
        }
        public static void LogWarning<T>(ICollection<T> _Objs, string _Color = black, string _Tag = "RTLog", bool _IsBold = false, float _Size = 12f)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < _Objs.Count; i++)
            {
                builder.AppendLine(_Objs.ElementAt<T>(i).ToString());
            }
            LogWarning(builder.ToString(), _Color, _Tag, _IsBold, _Size);
        }


        public static void LogWarning(string _Content = "", string _Color = black, string _Tag = "RTLog", bool _IsBold = false, float _Size = 12f)
        {
            if (_IsBold)
            {
                object[] args = new object[] { _Size, _Color, _Content, _Tag };
                Debug.LogWarning(string.Format("<b>[{3}]:</b> <b><size={0}><color=\"#{1}\">{2}</color></size></b>", args));
            }
            else
            {
                object[] objArray2 = new object[] { _Size, _Color, _Content, _Tag };
                Debug.LogWarning(string.Format("<b>[{3}]:</b> <size={0}><color=\"#{1}\">{2}</color></size>", objArray2));
            }
        }
    }
}

