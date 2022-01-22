using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Templatize.Templates;

public abstract partial class Template
{
    public ImmutableDictionary<object, object> Details
    {
        get => _details;
        set
        {
            if (value != _details)
            {
                OnDetailsUpdating(new(value));
            }
        }
    }

    public static implicit operator Layout(Template template)
    {
        Layout.Builder builder = template.ToContext().ToBuilder();
        builder.Templates.Add(template);
        return builder.ToContext();
    }

    public event EventHandler<DetailsUpdatingEventArgs>? DetailsUpdating;

    protected Template(IDictionary details)
    {
        switch (details)
        {
            case ImmutableDictionary<object, object> dictionary:
                _details = dictionary;
                break;

            case IDictionary<object, object> dictionary:
                _details = dictionary.ToImmutableDictionary();
                break;

            default:
                _details = details.Cast<DictionaryEntry>().ToImmutableDictionary(entry => entry.Key, entry => entry.Value)!;
                break;
        }
    }

    protected virtual void OnDetailsUpdating(DetailsUpdatingEventArgs args)
    {
        DetailsUpdating?.Invoke(this, args);
    }

    protected abstract Layout ToContext();

    private readonly ImmutableDictionary<object, object> _details;
}
