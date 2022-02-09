using System.Collections;
using System.Collections.Generic;

namespace Layoutize.Views;

public abstract class Tree : IEnumerable<Node>
{
    public abstract IEnumerator<Node> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    protected abstract View View { get; }
}
