using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Schemata;

public class FileEnumerator : IEnumerator<Connection.Segment>
{
    [AllowNull]
    public Connection.Segment Current { get; private set; }

    object IEnumerator.Current => Current;

    private bool IsEnumerated { get; set; }

    private Connection.Segment Entry { get; }

    public FileEnumerator(FileNetwork network)
    {
        Entry = new(network.Model);
    }

    public bool MoveNext()
    {
        if (!IsEnumerated)
        {
            Current = Entry;
            IsEnumerated = true;
            return true;
        }
        Current = null;
        return false;
    }

    public void Reset()
    {
        IsEnumerated = false;
    }

    public virtual void Dispose()
    {
        Entry.Dispose();
        GC.SuppressFinalize(this);
    }
}
