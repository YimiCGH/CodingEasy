using UnityEngine;
using System.Collections;
using System.Globalization;

namespace YimiTools.ColorUtil {
    public static class ColorEx
    {
        public const string black = "000000FF";
        public const string white = "FFFFFFFF";
        public static Color HexToColor(string _Hex)
        {
            byte num = byte.Parse(_Hex.Substring(0, 2), NumberStyles.HexNumber);
            byte num2 = byte.Parse(_Hex.Substring(2, 2), NumberStyles.HexNumber);
            byte num3 = byte.Parse(_Hex.Substring(4, 2), NumberStyles.HexNumber);
            byte num4 = byte.Parse(_Hex.Substring(6, 2), NumberStyles.HexNumber);
            float num5 = ((float)num) / 255f;
            float num6 = ((float)num2) / 255f;
            float num7 = ((float)num3) / 255f;
            return new Color(num5, num6, num7, ((float)num4) / 255f);
        }

        public static string ToHex(this Color _Color)
        {
            int num = Mathf.RoundToInt(_Color.r * 255f);
            int num2 = Mathf.RoundToInt(_Color.g * 255f);
            int num3 = Mathf.RoundToInt(_Color.b * 255f);
            int num4 = Mathf.RoundToInt(_Color.a * 255f);
            object[] args = new object[] { num, num2, num3, num4 };
            return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", args);
        }



    }

}
