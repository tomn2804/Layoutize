using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Schemata;

public class DirectoryPreorderEnumerator : IEnumerator<Connection.Segment>
{
    [AllowNull]
    public Connection.Segment Current { get; private set; }

    object IEnumerator.Current => Current;

    private DirectoryNetwork Network { get; }

    private Connection.Segment Parent { get; set; }

    public DirectoryPreorderEnumerator(DirectoryNetwork network)
    {
        Network = network;
        Reset();
    }

    private IEnumerator<IEnumerator<Connection.Segment>> ParentEnumerator { get; set; }

    private IEnumerator<Connection.Segment>? ChildEnumerator => ParentEnumerator.Current;

    private bool IsEnumerating { get; set; }

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

    public virtual void Dispose()
    {
        ParentEnumerator.Dispose();
        ChildEnumerator?.Dispose();
        Parent.Dispose();
        GC.SuppressFinalize(this);
    }
}
