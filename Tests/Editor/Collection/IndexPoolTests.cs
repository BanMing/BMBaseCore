using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using BMBaseCore.Entities;
using System;

namespace Tests
{
    public class IndexPoolTests
    {
        [Test]
        public void Create([Values(1, 100, Int16.MaxValue)] Int16 capacity)
        {
            IndexPool pool = new IndexPool(capacity);
            Assert.AreEqual(0, pool.Count);
            //Assert.AreEqual(Count(pool), pool.Count);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void IndexPoolTestsSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator IndexPoolTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
