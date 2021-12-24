using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Schemata;

public class DirectoryBfsEnumerator : IEnumerator<Connection.Segment>
{
    public IEnumerator<Connection.Segment> BasePosition { get; set; }
    public IEnumerator<Connection.Segment>? CurrentPosition { get; set; }

    [AllowNull]
    public Connection.Segment Current { get; private set; }

    object IEnumerator.Current => Current;

    private Connection.Segment Entry { get; }

    public DirectoryBfsEnumerator(DirectoryNetwork network)
    {
        Entry = new(network.Model);
        Children = network.Model.Children.Select(child => new Connection.Segment(child)).ToList();
        BasePosition = Children.GetEnumerator();
    }

    private List<Connection.Segment> Children { get; }

    public bool MoveNext()
    {
        if (CurrentPosition is null)
        {
            Current = Entry;
            CurrentPosition = Children.GetEnumerator();
            return true;
        }
        if (CurrentPosition.MoveNext())
        {
            Current = CurrentPosition.Current;
            return true;
        }
        BasePosition.Current?.Dispose();
        if (BasePosition.MoveNext())
        {
            CurrentPosition = BasePosition.Current.Model.Network.GetEnumerator();
            CurrentPosition.MoveNext();
            return MoveNext();
        }
        Entry.Dispose();
        Current = null;
        return false;
    }

    public void Reset()
    {
        BasePosition.Reset();
        CurrentPosition = null;
    }

    public virtual void Dispose()
    {
        BasePosition.Dispose();
        Entry.Dispose();
        GC.SuppressFinalize(this);
    }
}
