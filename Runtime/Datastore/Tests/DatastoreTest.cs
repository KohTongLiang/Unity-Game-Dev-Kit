using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace GameCore
{
    public class DatastoreTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void DatastoreTestSimplePasses()
        {
            // Use the Assert class to test conditions
            // Datastore datastore = new Datastore();
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator DatastoreTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
