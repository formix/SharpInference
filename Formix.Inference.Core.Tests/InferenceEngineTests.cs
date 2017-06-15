using Formix.Inference.Core.Tests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Formix.Inference.Core.Tests
{

    [TestClass]
    public class InferenceEngineTests
    {

        [TestMethod]
        public void TestLoad()
        {
            var engine = new InferenceEngine();
            engine.Load(new WeightModel());
            Assert.AreEqual(2, engine.Rules.Count);
        }


        [TestMethod]
        public void TestRun()
        {
            var engine = new InferenceEngine();
            engine.Load(new WeightModel());
            engine["kilograms"] = 5;
            engine.Run();
            Assert.AreEqual(5 * 2.2, engine["pounds"]);
        }

    }
}
