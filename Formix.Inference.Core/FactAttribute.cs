using System;

namespace Formix.Inference.Core
{
    /// <summary>
    /// Attibute to set to a method parameter to reference a fact with a 
    /// different name than the current parameter name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited=true)]
    public class FactAttribute : Attribute
    {
        /// <summary>
        /// The path of tha fact referenced by the parameter.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Creates the attribute with the given fact name.
        /// </summary>
        /// <param name="path">The path of the target fact. Start the path 
        /// with '/' to define an absolute path.</param>
        public FactAttribute(string path)
        {
            Path = path;
        }
    }
}
