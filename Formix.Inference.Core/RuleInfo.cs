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
            FactNames = CreateFactPaths(source.GetType(), method);
            Root = "";
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
        /// The root path of the current rule. That value is set when the 
        /// rule is added to a different place than the root of the engine.
        /// </summary>
        public string Root { get; set; }

        /// <summary>
        /// Gets the array of all fact requested by the rule.
        /// </summary>
        public string[] FactNames { get; }

        /// <summary>
        /// Execute the current rule within the context of the given engine.
        /// </summary>
        /// <param name="engine">The engine executing the rule.</param>
        public void Execute(InferenceEngine engine)
        {
            var parameters = new object[FactNames.Length];
            for (int i = 0; i < FactNames.Length; i++)
            {
                parameters[i] = engine[$"{Root}/{FactNames[i]}"];
            }

            var nameValPairs = (object[])Method.Invoke(Source, parameters);

            for (int i = 0; i < nameValPairs.Length; i += 2)
            {
                //string path = 
            }
        }


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
