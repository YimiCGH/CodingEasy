using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text;

namespace YimiTools.DataUtil
{
    public static class StringEx
    {
        public static bool CustomEndsWith(string a, string b) {
            int num = a.Length - 1;
            int num2 = b.Length - 1;
            while (((num >= 0) && (num2 >= 0)) && (a[num] == b[num2]))
            {
                num--;
                num2--;
            }
            return (((num2 < 0) && (a.Length >= b.Length)) || ((num < 0) && (b.Length >= a.Length)));
        }
        public static bool CustomStartsWith(string a, string b) {
            int length = a.Length;
            int num2 = b.Length;
            int num3 = 0;
            int num4 = 0;
            while (((num3 < length) && (num4 < num2)) && (a[num3] == b[num4]))
            {
                num3++;
                num4++;
            }
            return (((num4 == num2) && (length >= num2)) || ((num3 == length) && (num2 >= length)));
        }
        public static void RemoveEnd(this IList _List,int num = 1) {

            while (num > 0) {
                if (_List.Count > 0)
                {
                    _List.RemoveAt(_List.Count - 1);
                }
                else {
                    break;
                }
                num--;
            }
        }
        public static StringBuilder RemoveEnd(this StringBuilder _StringBuilder, int num = 1)
        {
            while (num > 0) {
                if (_StringBuilder.Length > 0)
                {
                    _StringBuilder.Remove(_StringBuilder.Length - 1, 1);
                }
                else
                {
                    break;
                }
                num--;
            }
            
            return _StringBuilder;

        }
    }
}