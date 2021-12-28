using System.Collections;
using System.Collections.Generic;

namespace Schemata;

public abstract class Tree : IEnumerable<Node>
{
    public abstract IEnumerator<Node> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    protected abstract Model Model { get; }
}
