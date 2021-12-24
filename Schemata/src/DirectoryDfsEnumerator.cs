using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Schemata;

public class DirectoryDfsEnumerator : IEnumerator<Connection.Segment>
{
    public IEnumerator<Connection.Segment> BasePosition { get; set; }
    public IEnumerator<Connection.Segment>? CurrentPosition { get; set; }

    [AllowNull]
    public Connection.Segment Current { get; private set; }

    object IEnumerator.Current => Current;

    private Connection.Segment Entry { get; }

    public DirectoryDfsEnumerator(DirectoryNetwork network)
    {
        Entry = new(network.Model);
        BasePosition = network.Model.Children.Select(child => new Connection.Segment(child)).GetEnumerator();
    }

    public bool MoveNext()
    {
        if (CurrentPosition is null)
        {
            if (Current is null)
            {
                Current = Entry;
                if (BasePosition.MoveNext())
                {
                    CurrentPosition = BasePosition.Current.Model.Network.GetEnumerator();
                }
                return true;
            }
            Current = null;
            return false;
        }
        if (CurrentPosition.MoveNext())
        {
            Current = CurrentPosition.Current;
            return true;
        }
        CurrentPosition.Dispose();
        if (BasePosition.MoveNext())
        {
            CurrentPosition = BasePosition.Current.Model.Network.GetEnumerator();
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
