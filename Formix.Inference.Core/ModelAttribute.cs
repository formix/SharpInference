using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formix.Inference.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelAttribute : Attribute
    {
        public ModelAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
