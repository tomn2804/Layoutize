using System.Collections.Generic;
using System.Collections;

namespace Schemata;

public partial class ChildrenSet
{
    private DirectoryModel Parent { get; }

    private SortedSet<Model> Children { get; } = new(new Model.Comparer());

    public int Count => Children.Count;

    public ChildrenSet(DirectoryModel parent)
    {
        Parent = parent;
    }

    public void Add(Template template)
    {
        Children.Add(new Workbench(template).BuildTo(Parent.FullName));
    }
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
