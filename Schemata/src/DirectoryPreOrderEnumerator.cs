using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Schemata;

public sealed class DirectoryPreorderEnumerator : IEnumerator<Connection>
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
        if (ChildEnumerator is not null && ChildEnumerator.MoveNext())
        {
            Current = ChildEnumerator.Current;
            return true;
        }
        ChildEnumerator?.Dispose();
        if (ParentEnumerator.MoveNext())
        {
            return MoveNext();
        }
        Parent.Dispose();
        Current = null;
        return false;
    }

    [MemberNotNull(nameof(Parent), nameof(ParentEnumerator))]
    public void Reset()
    {
        IsEnumerating = false;
        Parent = new(Network.Model);
        ParentEnumerator = Network.Model.Children.Select(child => child.Network.GetEnumerator()).GetEnumerator();
    }

    internal DirectoryPreorderEnumerator(DirectoryNetwork network)
    {
        Network = network;
        Reset();
    }

    private IEnumerator<Connection>? ChildEnumerator => ParentEnumerator.Current;

    private bool IsEnumerating { get; set; }

    private DirectoryNetwork Network { get; }

    private Connection Parent { get; set; }

    private IEnumerator<IEnumerator<Connection>> ParentEnumerator { get; set; }
}
