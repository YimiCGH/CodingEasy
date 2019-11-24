using YimiTools.SingletonEx;
using UnityEngine;
namespace Demo.Singleton {
    public class Mgr_Test : Singleton<Mgr_Test>
    {
        Mgr_Test() {
            Debug.Log("Init");
        }

        public void ShowMessage() {
            Debug.Log("This is Message");
        }
    }

}
