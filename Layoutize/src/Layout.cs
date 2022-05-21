using Layoutize.Elements;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Layoutize;

public abstract class Layout
{
    private protected Layout(IDictionary attributes)
    {
        Attributes = attributes switch
        {
            IImmutableDictionary<object, object?> dictionary => dictionary,
            IDictionary<object, object?> dictionary => dictionary.ToImmutableDictionary(),
            _ => attributes.Cast<DictionaryEntry>().ToImmutableDictionary(entry => entry.Key, entry => entry.Value),
        };
    }

    public IImmutableDictionary<object, object?> Attributes { get; }

    internal abstract Element CreateElement();
}
