using System;
using System.Collections.Generic;
using System.Reflection;


namespace Formix.Inference.Core
{
    /// <summary>
    /// Rule definition class.
    /// </summary>
    public class RuleInfo
    {
        /// <summary>
        /// RuleInfo constructor.
        /// </summary>
        /// <param name="source">The source object that holds the rule's method.</param>
        /// <param name="method">The method that contains the rule's code.</param>
        public RuleInfo(object source, MethodInfo method)
        {
            Source = source;
            Method = method;
            FactPaths = CreateFactPaths(source.GetType(), method);
        }

        /// <summary>
        /// Gets the name of the rule.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the rule method. That method is expected to return void or a 
        /// list of name-value pairs.
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// Gets the source object that holds the rule's method.
        /// </summary>
        public object Source { get; }

        /// <summary>
        /// Gets the array of all fact paths requested by the rule.
        /// </summary>
        public string[] FactPaths { get; }


        private string[] CreateFactPaths(Type type, MethodInfo method)
        {
            string modelName = GetModelName(type);
            string rulePath = GetRulePath(method);
            List<string> factPaths = CreateFactPathList(method, rulePath);
            return factPaths.ToArray();
        }

        private static List<string> CreateFactPathList(MethodInfo method, string rulePath)
        {
            List<string> factPaths = new List<string>();
            var parameters = method.GetParameters();
            foreach (var parameter in parameters)
            {
                var factPath = parameter.Name;
                var paramInfo = parameter.GetCustomAttribute<FactAttribute>();
                if (paramInfo != null)
                {
                    if (!paramInfo.Path.StartsWith("/"))
                    {
                        factPaths.Add($"{rulePath.TrimEnd('/')}/{paramInfo.Path}");
                    }
                    else
                    {
                        factPaths.Add(paramInfo.Path);
                    }
                }
            }

            return factPaths;
        }

        private static string GetRulePath(MethodInfo method)
        {
            var rulePath = ".";
            var Name = method.Name;
            var ruleAttr = method.GetCustomAttribute<RuleAttribute>();
            if (ruleAttr != null)
            {
                if (!string.IsNullOrWhiteSpace(ruleAttr.Name))
                {
                    Name = ruleAttr.Name;
                }
                rulePath = ruleAttr.Path;
            }

            return rulePath.Trim('/');
        }

        private static string GetModelName(Type type)
        {
            string modelName = type.Name;
            var modelAttr = type.GetTypeInfo().GetCustomAttribute<ModelAttribute>();
            if (modelAttr != null)
            {
                modelName = modelAttr.Name;
            }

            return modelName.Trim('/');
        }
    }
}
