using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;

namespace Formix.Inference.Core
{
    public class TreeNode
    {
        private string _name;
        private object _value;
        private ISet<RuleInfo> _rules;
        private TreeNode _parent;
        private IDictionary<string, TreeNode> _children;

        public TreeNode(string name = null, TreeNode parent = null)
        {
            _name = Name;
            _value = null;
            Rules1 = new HashSet<RuleInfo>();
            _parent = parent;
            _children = new Dictionary<string, TreeNode>();
        }

        public TreeNode Parent
        {
            get { return _parent; }
        }

        public string Name
        {
            get { return _name; }
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public ISet<RuleInfo> Rules
        {
            get { return Rules1; }
        }

        public IDictionary<string, TreeNode> Children
        {
            get { return _children; }
        }

        public ISet<RuleInfo> Rules1 { get => _rules; set => _rules = value; }

        public object this[string path]
        {
            get
            {
                var node = GetNode(path.Trim("/".ToCharArray()));
                if (node != null)
                {
                    return node.Value;
                }
                return null;
            }
            set
            {
                var node = GetNode(path.Trim("/".ToCharArray()), true);
                if (node != null)
                {
                    node.Value = value;
                }
            }
        }

        public void AddRule(RuleInfo rule)
        {
            foreach (var path in rule.FactPaths)
            {
                var node = GetNode(path, true);
                node.Rules.Add(rule);
            }
        }

        public TreeNode GetNode(string path, bool create = false)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return this;
            }

            var splittedPath = path.Split("/".ToCharArray(), 2);
            var childName = splittedPath[0];
            string remainingPath = null;
            if (splittedPath.Length > 1)
            {
                remainingPath = splittedPath[1];
            }

            if (childName == ".")
            {
                return GetNode(remainingPath, create);
            }

            if (childName == "..")
            {
                if (Parent == null)
                {
                    return null;
                }
                else
                {
                    return Parent.GetNode(remainingPath, create);
                }
            }
            
            if (!_children.ContainsKey(childName))
            {
                if (!create)
                {
                    return null;
                }
                _children.Add(childName, new TreeNode(childName, this));
            }

            var child = _children[childName];
            return child.GetNode(remainingPath, create);
        }
    }
}
