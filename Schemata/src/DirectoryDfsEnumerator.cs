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
    public Connection.Segment Current { get; set; }

    object? IEnumerator.Current => Current;

    private DirectoryNetwork Network { get; }

    public DirectoryDfsEnumerator(DirectoryNetwork network)
    {
        Network = network;
        BasePosition = Network.Model.Children.Select(child => new Connection.Segment(child)).GetEnumerator();
    }

    public bool MoveNext()
    {
        if (CurrentPosition is null)
        {
            if (Current is null)
            {
                Current = new(Network.Model);
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
        if (BasePosition.MoveNext())
        {
            CurrentPosition = BasePosition.Current.Model.Network.GetEnumerator();
            return MoveNext();
        }
        Current = null;
        return false;
    }

    public void Reset()
    {
        BasePosition.Reset();
        CurrentPosition = null;
    }

    public void Dispose()
    {
    }
}
