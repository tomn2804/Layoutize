using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Schemata;

public class FileEnumerator : IEnumerator<Connection.Segment>
{
    [AllowNull]
    public Connection.Segment Current { get; set; }

    object? IEnumerator.Current => Current;

    private FileNetwork Network { get; }

    private bool HasEnumerated { get; set; }

    public FileEnumerator(FileNetwork network)
    {
        Network = network;
    }

    public bool MoveNext()
    {
        if (!HasEnumerated)
        {
            Current = new(Network.Model);
            HasEnumerated = true;
            return true;
        }
        Current = null;
        return false;
    }

    public void Reset()
    {
        HasEnumerated = false;
    }

    public void Dispose()
    {
    }
}
