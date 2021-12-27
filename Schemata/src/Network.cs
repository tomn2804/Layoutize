using System.Collections;
using System.Collections.Generic;

namespace Schemata;

public abstract class Network : IEnumerable<Connection>
{
    public abstract IEnumerator<Connection> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    protected abstract Model Model { get; }
}
