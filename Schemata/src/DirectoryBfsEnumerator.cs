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
    public Connection.Segment Current { get; set; }

    object? IEnumerator.Current => Current;

    private DirectoryNetwork Network { get; }

    public DirectoryBfsEnumerator(DirectoryNetwork network)
    {
        Network = network;
        BasePosition = Network.Model.Children.Select(child => new Connection.Segment(child)).GetEnumerator();
    }

    public bool MoveNext()
    {
        if (CurrentPosition is null)
        {
            Current = new(Network.Model);
            CurrentPosition = Network.Model.Children.Select(child => new Connection.Segment(child)).GetEnumerator();
            return true;
        }
        if (CurrentPosition.MoveNext())
        {
            Current = CurrentPosition.Current;
            return true;
        }
        if (BasePosition.MoveNext())
        {
            CurrentPosition = BasePosition.Current.Model.Network.GetEnumerator();
            CurrentPosition.MoveNext();
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
