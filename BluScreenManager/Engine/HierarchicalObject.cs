using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BluEngine.Engine
{
    public class HierarchicalObject
    {
        /// <summary>
        /// The list of children belonging to this object. Do not modify this list directly!
        /// </summary>
        protected List<HierarchicalObject> Children
        {
            get { return children; }
        }
        private List<HierarchicalObject> children = new List<HierarchicalObject>();
        
        /// <summary>
        /// This object's parent in the hierarchy.
        /// Will throw an InvalidOperationException if an attempt to create a hierarchical loop is made.
        /// </summary>
        public HierarchicalObject Parent
        {
            get { return parent; }
            set
            {
                if (parent == value)
                    return;

                if (value != null && value.isAncestor(this))
                    throw new InvalidOperationException("You cannot set a HierarchicalObject's parent to one of it's children!");

                if (parent != null)
                    parent.children.Remove(this);
                parent = value;
                if (parent != null)
                    parent.children.Add(this);
            }
        }
        private HierarchicalObject parent;

        /// <summary>
        /// Create a HierarchicalObject instance.
        /// </summary>
        /// <param name="parent">The HierarchicalObject's parent.</param>
        public HierarchicalObject(HierarchicalObject parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Create a HierarchicalObject instance.
        /// </summary>
        public HierarchicalObject() : this(null) { }

        /// <summary>
        /// Check if a HierarchicalObject is an ancestor of this one.
        /// </summary>
        /// <param name="potentialAncestor">The value to check.</param>
        /// <returns>True if potentialAncestor was in the parent hierarchy of the current HierarchicalObject.</returns>
        public bool isAncestor(HierarchicalObject potentialAncestor)
        {
            if (potentialAncestor == null)
                return false;

            if (potentialAncestor == this)
                return true;

            return parent == null ? false : parent.isAncestor(potentialAncestor);
        }
    }
}
