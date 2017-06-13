using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Formix.Inference.Core.Tests
{
    [TestClass]
    public class TreeNodeTests
    {
        [TestMethod]
        public void TestSetValue()
        {
            var node = new TreeNode();
            node["/target/star"] = "Pollux";
            Assert.AreEqual("Pollux", node.Children["target"].Children["star"].Value);
        }

        [TestMethod]
        public void TestGetValue()
        {
            var node = new TreeNode();
            node["/target/star"] = "Pollux";
            Assert.AreEqual("Pollux", node["/target/star"]);
        }

        [TestMethod]
        public void TestGetValueSinglePeriod()
        {
            var node = new TreeNode();
            node["/target/./star/"] = "Pollux";
            Assert.AreEqual("Pollux", node.Children["target"].Children["star"].Value);
        }

        [TestMethod]
        public void TestGetValueDoublePeriod()
        {
            var node = new TreeNode();
            node["/target/star/../constellation"] = "Orion";
            Assert.AreEqual("Orion", node.Children["target"].Children["constellation"].Value);
        }


    }
}
