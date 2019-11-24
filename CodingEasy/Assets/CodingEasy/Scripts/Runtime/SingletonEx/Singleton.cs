using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YimiTools.SingletonEx
{
    public abstract class Singleton<T> where T : class
    {
        private static T m_instance;
        public static T Instance {
            get {
                if (m_instance == null) {
                    Type type = typeof(T);
                    //var constructor = type.GetConstructor(new Type[] { });
                    m_instance = Activator.CreateInstance(type,true) as T;
                }
                return m_instance;
            }
        }

        public virtual void Destroy() { }
    }

}
