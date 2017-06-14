using System;
using System.Collections.Generic;
using System.Text;

namespace Formix.Inference.Core
{
    public class InferenceEngine
    {
        private TreeNode _root;


        public InferenceEngine()
        {
            _root = new TreeNode();
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
                    node.Value = value;
                }
            }
        }


        public void Load(object model, string root = "")
        {

        }
    }
}
