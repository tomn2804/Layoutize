using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Templatize;

public abstract partial class View
{
    public partial class ChildrenSet
    {
        public ChildrenSet(DirectoryView parent)
        {
            Parent = parent;
        }

        public int Count => Children.Count;

        public void Add(Template template)
        {
            View child = new Factory(template).BuildTo(Parent.FullName);
            child.Parent = Parent;
            Children.Add(child);
        }

        public void AddRange(IEnumerable<Template> templates)
        {
            foreach (Template template in templates)
            {
                Children.Add(new Factory(template).FillTo(Parent));
            }
            foreach (Node node in Parent.Tree.Skip(1))
            {
                if (!node.View.IsMounted)
                {
                    node.Invoke(node.View.Activities[ActivityOption.Mount]);
                }
            }
        }

        private SortedSet<View> Children { get; } = new(new Comparer());

        private DirectoryView Parent { get; }
    }

    public partial class ChildrenSet : IEnumerable<View>
    {
        public IEnumerator<View> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
