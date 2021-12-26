using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Schemata;

public sealed class FileEnumerator : IEnumerator<Connection>
{
    [AllowNull]
    public Connection Current { get; private set; }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
        Parent.Dispose();
    }

    public bool MoveNext()
    {
        if (!IsEnumerated)
        {
            Current = Parent;
            IsEnumerated = true;
            return true;
        }
        Current = null;
        return false;
    }

    [MemberNotNull(nameof(Parent))]
    public void Reset()
    {
        IsEnumerated = false;
        Parent = new(Network.Model);
    }

    internal FileEnumerator(FileNetwork network)
    {
        Network = network;
        Reset();
    }

    private bool IsEnumerated { get; set; }

    private FileNetwork Network { get; }

    private Connection Parent { get; set; }
}
