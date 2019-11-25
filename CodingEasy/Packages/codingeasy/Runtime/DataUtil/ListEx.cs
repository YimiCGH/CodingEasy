using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace YimiTools.DataUtil
{
    public static class ListEx
    {
        public static T Pop<T>(this List<T> list)
        {
            int index = list.Count - 1;
            T local = list[index];
            list.RemoveAt(index);
            return local;
        }
        public static void Push<T>(this List<T> list, T _Item)
        {
            list.Add(_Item);
        }
    }
}