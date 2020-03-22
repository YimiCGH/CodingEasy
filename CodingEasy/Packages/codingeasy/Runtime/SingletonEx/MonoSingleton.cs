using UnityEngine;
using System.Collections;
namespace YimiTools.SingletonEx
{
    public abstract class MonoSingleton<T> : MonoInstance where T : class
    {
        private static T _instance;
        public static T Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType(typeof(T)) as T;
                }
                return _instance;
            }
        }

        private void OnDestroy()
        {
            Destroy();
        }
    }

    public abstract class MonoInstance : MonoBehaviour {

        protected abstract void Init();
        protected abstract void Destroy();
    }
}