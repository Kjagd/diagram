using System;
using System.Linq;
using Diagram;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DiagramTool.ViewModel;

namespace UnitTest
{
    [TestClass]
    public class MainViewModelTest
    {
        private readonly MainViewModel mvm = new MainViewModel();

        [TestMethod]
        public void TestAddClass()
        {
            int classCount = mvm.Klasses.Count;

            mvm.NewClassCommand.Execute(null);

            int newClassCount = mvm.Klasses.Count;
            Assert.AreEqual(newClassCount, classCount + 1);

            Klass klass = mvm.Klasses[classCount];
            Assert.IsNotNull(klass);
        }

        [TestMethod]
        public void TestDeleteClass()
        {
            TestAddClass();
            int classCount = mvm.Klasses.Count;

            Klass klass = mvm.Klasses[classCount-1];

            mvm.SelectKlass(klass);
            mvm.DeleteClassCommand.Execute(null);

            Assert.AreEqual(classCount - 1, mvm.Klasses.Count);
        }

        [TestMethod]
        public void TestCopyPaste()
        {
            TestAddClass();
            int classCount = mvm.Klasses.Count;
            var klass = mvm.Klasses.First();
            klass.Name = "Test1";

            mvm.SelectKlass(klass);
            mvm.CopyClassCommand.Execute(null);
            mvm.PasteClassCommand.Execute(null);

            Assert.AreEqual(classCount + 1, mvm.Klasses.Count);

            var pastedKlass = mvm.Klasses.Last();
            Assert.AreEqual(pastedKlass, klass);
            Assert.AreNotSame(pastedKlass, klass);
        }
    }
}
