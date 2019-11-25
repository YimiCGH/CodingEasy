using System.IO;
using NUnit.Framework;
using UnityEngine.TestTools;
using Demo.Singleton;

namespace Tests {
    public class CodingEasy_Test
    {
        [Test]
        public void Singleton_Test()
        {
            // Use the Assert class to test conditions

            try
            {
                Mgr_Test.Instance.ShowMessage();
            }
            catch (System.Exception e)
            {
                Assert.IsNotEmpty(e.Message, e.Message);
            }
        }
    }

}
