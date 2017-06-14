using System;
using System.Collections.Generic;
using System.Text;

namespace Formix.Inference.Core
{
    /// <summary>
    /// The Fact class is used to update a fact from the InferenceEngine 
    /// after a rule was executed.
    /// </summary>
    public class Fact
    {
        /// <summary>
        /// Creates a Fact instance with the given name and value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public Fact(string name, object value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// The name of the fact. Should be in the form of a path.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The value of the fact.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// This method takes an array of object that contains an 
        /// alternating string-object pairs. Each pair are then used to 
        /// create a Fact object, combined into an array and returned. If the
        /// nameValuePairs array contains an odd number of elements, the last
        /// element is ignored.
        /// </summary> 
        /// <param name="nameValuPairs">An alternating array of name-value pairs.</param>
        /// <returns>An array of Facts.</returns>
        public static Fact[] Combine(params object[] nameValuPairs)
        {
            Fact[] facts = new Fact[nameValuPairs.Length / 2];
            for (int i = 0; i < nameValuPairs.Length; i += 2)
            {
                string name = (string)nameValuPairs[i];
                object value = nameValuPairs[i + 1];
                facts[i / 2] = new Fact(name, value);
            }
            return facts;
        }
    }
}
