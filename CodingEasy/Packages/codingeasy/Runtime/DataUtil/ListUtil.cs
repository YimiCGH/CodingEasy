using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ListUtil
{
    public static void RandomSort<T>(this List<T> list) {
        var random = new System.Random();
        var sortList = ListPool<T>.Get();
        {
            for (int i = 0; i < list.Count; i++)
            {
                sortList.Insert(random.Next(sortList.Count), list[i]);
            }
            list.Clear();
            list.AddRange(sortList);
        }
        ListPool<T>.Add(sortList);
    }
}
