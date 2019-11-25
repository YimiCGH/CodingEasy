using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace YimiTools.DataUtil
{
    public static class DictionaryEx
    {
        public static void Add<K, V>(this Dictionary<K, V> _Src, Dictionary<K, V> _Add)
        {
            foreach (KeyValuePair<K, V> pair in _Add)
            {
                _Src.TryAdd<K, V>(pair.Key, pair.Value);
            }
        }
        public static K[] GetKeys<K, V>(this Dictionary<K, V> _Src)
        {
            K[] array = new K[_Src.Count];
            _Src.Keys.CopyTo(array, 0);
            return array;
        }

        public static V[] GetValues<K, V>(this Dictionary<K, V> _Src)
        {
            V[] array = new V[_Src.Count];
            _Src.Values.CopyTo(array, 0);
            return array;
        }

        public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> _Dict, TKey _Key, TValue _Value)
        {
            if (_Dict == null)
            {
                Debug.LogError(new NullReferenceException().Message);
                return false;
            }
            if (!_Dict.ContainsKey(_Key))
            {
                _Dict.Add(_Key, _Value);
                return true;
            }
            return false;
        }
    }
}