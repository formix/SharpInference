using System.Collections.Generic;

namespace Formix.Inference.Core
{
    /// <summary>
    /// The TreeNode class defines the structure of the facts within the 
    /// inference engine.
    /// </summary>
    public class TreeNode
    {
        private string _name;
        private object _value;
        private ISet<RuleInfo> _rules;
        private TreeNode _parent;
        private IDictionary<string, TreeNode> _children;

        /// <summary>
        /// Creates a new TreeNode object.
        /// </summary>
        /// <param name="name">Optional, defaults to Null. Sets the name of 
        /// the node.</param>
        /// <param name="parent">Optional, defaults to null. Sets the parent 
        /// of the current node.</param>
        public TreeNode(string name = null, TreeNode parent = null)
        {
            _name = Name;
            _value = null;
            _rules = new HashSet<RuleInfo>();
            _parent = parent;
            _children = new Dictionary<string, TreeNode>();
        }

        /// <summary>
        /// The parent of the TreeNode.
        /// </summary>
        public TreeNode Parent
        {
            get { return _parent; }
        }

        /// <summary>
        /// The name of the TreeNode.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// The value stored in the TreeNode.
        /// </summary>
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Rules associated to the current node.
        /// </summary>
        public ISet<RuleInfo> Rules
        {
            get { return _rules; }
        }

        /// <summary>
        /// Indexed children nodes by name.
        /// </summary>
        public IDictionary<string, TreeNode> Children
        {
            get { return _children; }
        }


        /// <summary>
        /// Gets or sets the value of the target child node. The indexer 
        /// alloes accessing to any child TreeNode using a path notation 
        /// separated with slashes "/". Using the ".." goes up one node. 
        /// Using the "." Refers to the current node. Mostly for test purpose.
        /// </summary>
        /// <param name="path">The path to the child node from the current 
        /// TreeNode.</param>
        /// <returns>The value of the target child node. If the child node 
        /// doesn't exist, returns null.</returns>
        public object this[string path]
        {
            get
            {
                var node = GetNode(path.Trim('/'));
                if (node != null)
                {
                    return node.Value;
                }
                return null;
            }
            set
            {
                var node = GetNode(path.Trim('/'), true);
                if (node != null)
                {
                    node.Value = value;
                }
            }
        }

        /// <summary>
        /// Add the given rule to all nodes targetted by the facts it uses. 
        /// If the targetted node path doesn't exists, creates it.
        /// </summary>
        /// <param name="rule">The rule to set to the child nodes.</param>
        public void AddRule(RuleInfo rule)
        {
            foreach (var path in rule.Facts)
            {
                var node = GetNode(path, true);
                node.Rules.Add(rule);
            }
        }

        /// <summary>
        /// Get a node targetted bu the give path.
        /// </summary>
        /// <param name="path">The path to the desired node.</param>
        /// <param name="create">Optional, defaults to false. If true, 
        /// creates the nodes along the specified path if it doesn't 
        /// exist.</param>
        /// <returns>The node targetted by the path parameter or null if the 
        /// create parameter is false and the path doesn't exists.</returns>
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
