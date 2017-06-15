using System;
using System.Collections.Generic;
using System.Text;

namespace Formix.Inference.Core.Tests.TestModels
{
    public class WeightModel
    {
        public Fact[] UpdatePounds(double kilograms)
        {
            var pounds = kilograms * 2.2;
            return Fact.Combine("pounds", pounds);
        }

        public Fact[] UpdateKilograms(double pounds)
        {
            var kilograms = pounds / 2.2;
            return Fact.Combine("kilograms", kilograms);
        }
    }
}
