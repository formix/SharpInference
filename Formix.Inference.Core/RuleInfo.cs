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
            Facts = CreateFactPaths(source.GetType(), method);
        }

        /// <summary>
        /// Gets the name of the rule. It is always in the form {Root}/{Path}/{Method Name}.
        /// </summary>
        public string Name { get { return Method.Name; } }

        /// <summary>
        /// The name of the model of that rule.
        /// </summary>
        public string ModelName { get; set; }

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
        /// Gets the array of all fact requested by the rule.
        /// </summary>
        public string[] Facts { get; }

        /// <summary>
        /// Execute the current rule within the context of the given engine.
        /// </summary>
        /// <param name="engine">The engine executing the rule.</param>
        public void Execute(InferenceEngine engine)
        {
            var parameters = new object[Facts.Length];
            for (int i = 0; i < Facts.Length; i++)
            {
                parameters[i] = engine[Facts[i]];
            }

            var facts = (Fact[])Method.Invoke(Source, parameters);

            if (facts != null)
            {
                foreach (var fact in facts)
                {
                    engine[fact.Name] = fact.Value;
                }
            }
        }


        private string[] CreateFactPaths(Type type, MethodInfo method)
        {
            ModelName = GetModelName(type);
            List<string> factPaths = CreateFactPathList(method);
            return factPaths.ToArray();
        }

        private List<string> CreateFactPathList(MethodInfo method)
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
                        factPath = /* root and prefix */ paramInfo.Path;
                    }
                    else
                    {
                        factPath = paramInfo.Path;
                    }
                }
                factPaths.Add(factPath);
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
