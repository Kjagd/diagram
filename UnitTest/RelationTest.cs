using System;
using Diagram;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class RelationTest
    {
        [TestMethod]
        public void TestSet()
        {
            var k1 = new Klass("Test1");
            var k2 = new Klass("Test2");

            //Type doesn't matter
            var relation = new Relation(Relation.Type.Composition);
            relation.Set(k1, k2);

            Assert.AreSame(k1, relation.From);
            Assert.AreSame(k2, relation.To);
            Assert.IsTrue(k1.Relations.Contains(relation));
            Assert.IsTrue(k2.Relations.Contains(relation));
        }

        [TestMethod]
        public void TestUnSet()
        {
            var k1 = new Klass("Test1");
            var k2 = new Klass("Test2");

            //Type doesn't matter
            var relation = new Relation(Relation.Type.Composition);
            relation.Set(k1, k2);

            relation.UnSet();
            Assert.IsNull(relation.From);
            Assert.IsNull(relation.To);

            Assert.IsFalse(k1.Relations.Contains(relation));
            Assert.IsFalse(k2.Relations.Contains(relation));
        }
    }
}
