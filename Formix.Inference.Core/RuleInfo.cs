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
        private string _path;

        /// <summary>
        /// RuleInfo constructor.
        /// </summary>
        /// <param name="source">The source object that holds the rule's method.</param>
        /// <param name="method">The method that contains the rule's code.</param>
        public RuleInfo(object source, MethodInfo method)
        {
            _path = null;
            Source = source;
            Method = method;
            FactNames = CreateFactPaths(source.GetType(), method);
            Root = "";
        }

        /// <summary>
        /// The root path of the current rule. That value is set when the 
        /// rule is added to a different place than the root of the engine.
        /// </summary>
        public string Root { get; set; }

        /// <summary>
        /// Gets the path of the rule. It is always in the form {Root}/{Path}.
        /// </summary>
        public string Path
        {
            get
            {
                if (_path == null)
                {
                    _path = GetRulePath(Method);
                }
                return $"{Root}/{_path}".Trim('/');
            }
        }

        /// <summary>
        /// Gets the name of the rule. It is always in the form {Root}/{Path}/{Method Name}.
        /// </summary>
        public string Name
        {
            get
            {
                return $"{Root}/{Path}/{Method.Name}".TrimStart('/');
            }
        }


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

            var facts = (Fact[])Method.Invoke(Source, parameters);

            if (facts != null)
            {
                foreach (var fact in facts)
                {

                }
            }
        }


        private string[] CreateFactPaths(Type type, MethodInfo method)
        {
            string modelName = GetModelName(type);
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
                        factPaths.Add($"{Path}/{paramInfo.Path}");
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
