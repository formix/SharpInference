using System;

namespace Formix.Inference.Core
{
    /// <summary>
    /// Defines a methd as a rule. You can set the optional property "Name" 
    /// to give the rule a different name than the method name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited=true)]
    public class RuleAttribute : Attribute
    {
        private string _path;

        /// <summary>
        /// Creates a new RuleAttribute.
        /// </summary>
        public RuleAttribute()
        {
            _path = "/";
        }

        /// <summary>
        /// Get or set the base path of the rule. Default to "/". Facts
        /// reference name can be made relative to that base path 
        /// (FactAttribute.Name).
        /// </summary>
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                if (!value.StartsWith("/"))
                {
                    _path = $"/{value}";
                }
                else
                {
                    _path = value;
                }
            }
        }

        /// <summary>
        /// Get or set the name of the rule.
        /// </summary>
        public string Name { get; set; }
    }
}
