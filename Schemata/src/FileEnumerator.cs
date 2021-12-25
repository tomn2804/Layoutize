using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Schemata;

public class FileEnumerator : IEnumerator<Connection>
{
    [AllowNull]
    public Connection Current { get; private set; }

    object IEnumerator.Current => Current;

    private bool IsEnumerated { get; set; }

    private FileNetwork Network { get; }

    private Connection Parent { get; set; }

    public FileEnumerator(FileNetwork network)
    {
        Network = network;
        Reset();
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

    public virtual void Dispose()
    {
        Parent.Dispose();
        GC.SuppressFinalize(this);
    }
}
