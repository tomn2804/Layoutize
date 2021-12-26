using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Schemata;

public sealed class DirectoryLevelOrderEnumerator : IEnumerator<Connection>
{
    [AllowNull]
    public Connection Current { get; private set; }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
        ChildEnumerator?.Dispose();
        Parent.Dispose();
        ParentEnumerator.Dispose();
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

    internal DirectoryLevelOrderEnumerator(DirectoryNetwork network)
    {
        Network = network;
        Children.EnsureCapacity(Network.Model.Children.Count);
        Reset();
    }

    private IEnumerator<Connection>? ChildEnumerator => ParentEnumerator.Current;

    private Queue<IEnumerator<Connection>> Children { get; } = new();

    private bool IsEnumerating { get; set; }

    private DirectoryNetwork Network { get; }

    private Connection Parent { get; set; }

    private IEnumerator<IEnumerator<Connection>> ParentEnumerator { get; set; }
}
