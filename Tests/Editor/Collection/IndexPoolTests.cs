using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using BMBaseCore.Entities;
using System;

namespace Tests
{
    public class IndexPoolTests
    {
        private struct TestStruct
        {
            public int X;
            public string Foo;
            public float Bar;

            public TestStruct(int x, string foo, float bar)
            {
                X = x;
                Foo = foo;
                Bar = bar;
            }

            public override string ToString()
            {
                return string.Format("TestStruct {{X={0}, Foo={1}, Bar={2}}}", X, Foo, Bar);
            }
        }

        [Test]
        public void Create([Values(1, 100, Int16.MaxValue)] Int16 capacity)
        {
            IndexPool pool = new IndexPool(capacity);
            Assert.AreEqual(0, pool.Count);
            //Assert.AreEqual(Count(pool), pool.Count);
        }

        [Test]
        public void NotInvalidOperationRelease()
        {
            IndexPool pool = new IndexPool(10);
            int e1 = pool.Reserve();
            int e2 = pool.Reserve();

            foreach (Int16 item in pool)
            {
                pool.Release(item);
            }

            Assert.That(pool.Count == 0);
        }

        [Test]
        public void Reserve()
        {
            IndexPool pool = new IndexPool(10);
            TestStruct[] data = new TestStruct[10];

            int e1 = pool.Reserve();
            Assert.That(e1 >= 0);
            Assert.AreEqual(1, pool.Count);
            data[e1].X = 1;
            data[e1].Foo = "Two";
            data[e1].Bar = 3.45f;

            int e3 = pool.Reserve();
            int e4 = pool.Reserve();
            pool.Release((Int16)e3);
            int e5 = pool.Reserve();
            int e6 = pool.Reserve();
            pool.Release((Int16)e4);
            pool.Release((Int16)e5);
            pool.Release((Int16)e6);

            int e2 = pool.Reserve();
            Assert.That(e2 >= 0);
            //Assert.AreEqual(2, pool.Count);
            data[e2].X = 2;
            data[e2].Foo = "Three";
            data[e2].Bar = 4.56f;

            foreach (var item in pool)
            {
                UnityEngine.Debug.Log(item);
                UnityEngine.Debug.Log(data[item].ToString());
            }

        }
    }
}
