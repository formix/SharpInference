using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Reflection;


namespace Formix.Inference.Core
{
    public class RuleInfo
    {
        public RuleInfo(object target, MethodInfo method)
        {
            Target = target;
            Method = method;
            FactPaths = CreateFactPaths(target.GetType(), method);
        }

        public string Name { get; private set; }
        public MethodInfo Method { get; }
        public object Target { get; }
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
