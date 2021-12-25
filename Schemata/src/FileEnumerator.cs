using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Schemata;

public class FileEnumerator : IEnumerator<Connection>
{
    public FileEnumerator(FileNetwork network)
    {
        Network = network;
        Reset();
    }

    [AllowNull]
    public Connection Current { get; private set; }

    object IEnumerator.Current => Current;

    public virtual void Dispose()
    {
        Parent.Dispose();
        GC.SuppressFinalize(this);
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

    private bool IsEnumerated { get; set; }

    private FileNetwork Network { get; }

    private Connection Parent { get; set; }
}
