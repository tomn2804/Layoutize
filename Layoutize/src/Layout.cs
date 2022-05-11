using Layoutize.Attributes;
using Layoutize.Elements;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Layoutize;

public abstract class Layout
{
    private protected Layout(IEnumerable attributes)
    {
        Attributes = attributes switch
        {
            IImmutableDictionary<object, object> dictionary => dictionary,
            IEnumerable<KeyValuePair<object, object>> entries => entries.ToImmutableDictionary(),
            _ => attributes.Cast<DictionaryEntry>().ToImmutableDictionary(entry => entry.Key, entry => entry.Value!),
        };
        Name.RequireOf(this);
    }

    public IImmutableDictionary<object, object> Attributes { get; }

    internal abstract Element CreateElement();
}
