using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml;
using Lithnet.MetadirectoryServices;
using Lithnet.Transforms;
using Microsoft.MetadirectoryServices;
using Lithnet.Common.ObjectModel;
using System.Linq;

namespace Lithnet.Transforms.UnitTests
{
    /// <summary>
    ///This is a test class for RegexReplaceTransformTest and is intended
    ///to contain all RegexReplaceTransformTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StringReplaceTransformTest
    {
        [ClassInitialize()]
        public static void InitializeTest(TestContext testContext)
        {
            UnitTestControl.Initialize();
        }

        [TestMethod()]
        public void TestSerialization()
        {
            UniqueIDCache.ClearIdCache();
            StringReplaceTransform transformToSeralize = new StringReplaceTransform();
            transformToSeralize.LookupItems.Add(new LookupItem() { CurrentValue = "test1", NewValue = "test2" });
            transformToSeralize.LookupItems.Add(new LookupItem() { CurrentValue = "test3", NewValue = "test4" });
            transformToSeralize.IgnoreCase = true;
            transformToSeralize.ID = "test001";
            UniqueIDCache.ClearIdCache();

            StringReplaceTransform deserializedTransform = (StringReplaceTransform)UnitTestControl.XmlSerializeRoundTrip<Transform>(transformToSeralize);

            Assert.AreEqual(transformToSeralize.ID, deserializedTransform.ID);
            Assert.AreEqual(transformToSeralize.IgnoreCase, deserializedTransform.IgnoreCase);
            CollectionAssert.AreEqual(transformToSeralize.LookupItems, deserializedTransform.LookupItems, new LookupItemComparer());
        }

        [TestMethod()]
        public void StringReplaceTransformStringElementTest()
        {
            StringReplaceTransform transform = new StringReplaceTransform();
            transform.LookupItems.Add(new LookupItem() { CurrentValue = "test1", NewValue = "test2" });
            transform.LookupItems.Add(new LookupItem() { CurrentValue = "test3", NewValue = "test4" });

            this.ExecuteTestString(transform, "test1", "test2");
        }

        [TestMethod()]
        public void StringReplaceSubStringTest1()
        {
            StringReplaceTransform transform = new StringReplaceTransform();
            transform.IgnoreCase = true;
            transform.LookupItems.Add(new LookupItem() { CurrentValue = "A", NewValue = "b" });
            transform.LookupItems.Add(new LookupItem() { CurrentValue = "c", NewValue = "d" });

            this.ExecuteTestString(transform, "abcd", "bbdd");
        }

        [TestMethod()]
        public void StringReplaceSubStringTest3()
        {
            StringReplaceTransform transform = new StringReplaceTransform();
            transform.IgnoreCase = false;
            transform.LookupItems.Add(new LookupItem() { CurrentValue = "A", NewValue = "b" });
            transform.LookupItems.Add(new LookupItem() { CurrentValue = "c", NewValue = "d" });

            this.ExecuteTestString(transform, "abcd", "abdd");
        }

        [TestMethod()]
        public void StringReplaceSubStringTest2()
        {
            StringReplaceTransform transform = new StringReplaceTransform();
            transform.LookupItems.Add(new LookupItem() { CurrentValue = "ß", NewValue = "ss" });
            transform.LookupItems.Add(new LookupItem() { CurrentValue = "á", NewValue = "a" });

            this.ExecuteTestString(transform, "Einbahnstraße", "Einbahnstrasse");
            this.ExecuteTestString(transform, "ábcdeá", "abcdea");
        }


        private void ExecuteTestString(StringReplaceTransform transform, string sourceValue, string expectedValue)
        {
            string outValue = transform.TransformValue(sourceValue).FirstOrDefault() as string;

            Assert.AreEqual(expectedValue, outValue);
        }
    }
}
