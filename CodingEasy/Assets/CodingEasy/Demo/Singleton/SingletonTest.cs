using UnityEngine;
using System.Collections;
namespace Demo.Singleton
{
    public class SingletonTest : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            Mgr_Test.Instance.ShowMessage();
        }

    }
}