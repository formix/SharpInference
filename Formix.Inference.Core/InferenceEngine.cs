using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Formix.Inference.Core
{
    /// <summary>
    /// The InferenceEngine class is used to store a hierarchical set of 
    /// facts and execute rules to maintain its states in a valid state.
    /// </summary>
    public class InferenceEngine
    {
        private TreeNode _root;
        private Dictionary<string, RuleInfo> _rules;
        private LinkedHashSet<RuleInfo> _agenda;

        /// <summary>
        /// Instanciates an InferenceEngine object.
        /// </summary>
        public InferenceEngine()
        {
            _root = new TreeNode();
            _rules = new Dictionary<string, RuleInfo>();
            _agenda = new LinkedHashSet<RuleInfo>();
            MaxLoop = 100;
        }

        public int MaxLoop { get; set; }

        /// <summary>
        /// List of active rules in this InferenceEngine instance.
        /// </summary>
        public Dictionary<string, RuleInfo> Rules
        {
            get { return _rules; }
        }

        /// <summary>
        /// Gets or sets the value of the target fact. The indexer 
        /// alloes accessing to any fact using a path notation 
        /// separated with slashes "/". Using the ".." goes up one node. 
        /// Using the "." Refers to the current node.
        /// </summary>
        /// <param name="path">The path to the fact from the current 
        /// root node.</param>
        /// <returns>The value of the target fact. If the fact 
        /// doesn't exist, returns null.</returns>
        public object this[string path]
        {
            get
            {
                var node = _root.GetNode(path.Trim('/'));
                if (node != null)
                {
                    return node.Value;
                }
                return null;
            }
            set
            {
                var node = _root.GetNode(path.Trim('/'), true);
                if (node != null)
                {
                    if (!Equals(node.Value, value))
                    {
                        node.Value = value;
                        _agenda.UnionWith(node.Rules);
                    }
                }
            }
        }

        /// <summary>
        /// Loads a model into the engine under the optional root.
        /// </summary>
        /// <param name="model">The model to load in the engine.</param>
        /// <param name="root">Optional, defaults to empty string. The root 
        /// where to put the model.</param>
        public void Load(object model, string root = "")
        {
            var objectMethods = new HashSet<string>(
                typeof(object).GetMethods().Select(m => m.Name));
            var methods = model.GetType().GetMethods().Where(
                m => !objectMethods.Contains(m.Name));
            foreach (var method in methods)
            {
                var rule = new RuleInfo(model, method);
                _rules.Add(rule.Name, rule);
                _root.AddRule(rule);
            }
        }

        /// <summary>
        /// Executes the inference engine.
        /// </summary>
        public void Run()
        {
            int loop = 0;
            while (_agenda.Count > 0 && loop < MaxLoop)
            {
                var agenda = _agenda;
                _agenda = new LinkedHashSet<RuleInfo>();
                foreach (var rule in agenda)
                {
                    rule.Execute(this);
                }
                loop++;
            }
        }
    }
}
