using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace YimiTools.DataUtil
{
    public static class ListQueueEx
    {
        public static T Dequeue<T>(this List<T> list)
        {
            T local = list[0];
            list.RemoveAt(0);
            return local;
        }
        public static void Enqueue<T>(this List<T> list, T _Item)
        {
            list.Add(_Item);
        }
        public static T Pick<T>(this List<T> list) {

            return list[0];
        }
    }
}