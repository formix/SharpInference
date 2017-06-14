using System;

namespace Formix.Inference.Core
{
    /// <summary>
    /// Give a model a name. If the model class doesn't have a ModelAttribute 
    /// set, the class name is used as the model name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelAttribute : Attribute
    {
        /// <summary>
        /// ModelAttribute constructor.
        /// </summary>
        /// <param name="name">The name of the model.</param>
        public ModelAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the name of the model.
        /// </summary>
        public string Name { get; private set; }
    }
}
