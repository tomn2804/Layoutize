using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Collections;

namespace Schemata;

public class DirectoryModel : Model
{
    public ChildrenSet Children { get; }

    public override DirectoryLevelOrderTree Tree { get; }

    public DirectoryModel(Blueprint blueprint)
        : base(blueprint)
    {
        Children = new(this);
        Tree = new(this);
    }

    public virtual void Create()
    {
        Directory.CreateDirectory(FullName);
    }
}

public partial class ChildrenSet
{
    private DirectoryModel Parent { get; }

    private SortedSet<Model> Children { get; } = new(new Model.Comparer());

    public int Count => Children.Count;

    public ChildrenSet(DirectoryModel parent)
    {
        Parent = parent;
    }

    public void Add(Blueprint blueprint)
    {
        Children.Add(new Workbench(blueprint).BuildTo(Parent.FullName));
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
