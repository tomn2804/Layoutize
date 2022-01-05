using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Templata;

public abstract partial class Model
{
    public partial class ChildrenSet
    {
        public ChildrenSet(DirectoryModel parent)
        {
            Parent = parent;
        }

        public int Count => Children.Count;

        public void Add(Template template)
        {
            Model child = new Workbench(template).BuildTo(Parent.FullName);
            child.Parent = Parent;
            Children.Add(child);
        }

        public void AddRange(IEnumerable<Template> templates)
        {
            foreach (Template template in templates)
            {
                Children.Add(new Workbench(template).FillTo(Parent));
            }
            foreach (Node node in Parent.Tree.Skip(1))
            {
                if (!node.Model.IsMounted)
                {
                    node.Invoke(node.Model.Activities[ActivityOption.Mount]);
                }
            }
        }

        private SortedSet<Model> Children { get; } = new(new Comparer());

        private DirectoryModel Parent { get; }
    }

    public partial class ChildrenSet : IEnumerable<Model>
    {
        public IEnumerator<Model> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
