using UnityEngine;
using System.Collections;
using System.Reflection;

namespace YimiTools.GCUtil
{
    public static class GCHelper
    {
        public static void Release(this object _Obj)
        {
            FieldInfo[] fields = _Obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            foreach (FieldInfo info in fields)
            {
                if (!info.FieldType.IsValueType)
                {
                    info.SetValue(_Obj, null);
                }
            }
        }
    }
}
