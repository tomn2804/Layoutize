using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Schemata;

public class DirectoryLevelOrderEnumerator : IEnumerator<Connection>
{
    public DirectoryLevelOrderEnumerator(DirectoryNetwork network)
    {
        Network = network;
        Children.EnsureCapacity(Network.Model.Children.Count);
        Reset();
    }

    [AllowNull]
    public Connection Current { get; private set; }

    object IEnumerator.Current => Current;

    public virtual void Dispose()
    {
        ChildEnumerator?.Dispose();
        Parent.Dispose();
        ParentEnumerator.Dispose();
        GC.SuppressFinalize(this);
    }

    public bool MoveNext()
    {
        if (!IsEnumerating)
        {
            Current = Parent;
            IsEnumerating = true;
            return true;
        }
        if (ParentEnumerator.MoveNext() && ChildEnumerator!.MoveNext())
        {
            Children.Enqueue(ChildEnumerator);
            Current = ChildEnumerator.Current;
            return true;
        }
        if (Children.Any())
        {
            if (Children.Peek().MoveNext())
            {
                Current = Children.Peek().Current;
                return true;
            }
            Children.Dequeue().Dispose();
            return MoveNext();
        }
        Parent.Dispose();
        Current = null;
        return false;
    }

    [MemberNotNull(nameof(Parent), nameof(ParentEnumerator))]
    public void Reset()
    {
        Children.Clear();
        IsEnumerating = false;
        Parent = new(Network.Model);
        ParentEnumerator = Network.Model.Children.Select(child => child.Network.GetEnumerator()).GetEnumerator();
    }

    private IEnumerator<Connection>? ChildEnumerator => ParentEnumerator.Current;

    private Queue<IEnumerator<Connection>> Children { get; } = new();

    private bool IsEnumerating { get; set; }

    private DirectoryNetwork Network { get; }

    private Connection Parent { get; set; }

    private IEnumerator<IEnumerator<Connection>> ParentEnumerator { get; set; }
}
