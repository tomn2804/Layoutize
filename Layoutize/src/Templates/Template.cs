using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Layoutize.Templates;

public abstract partial class Template
{
    public ImmutableDictionary<object, object> Attributes
    {
        get => _attributes;
        set
        {
            if (value != _attributes)
            {
                OnAttributesUpdating(new(value));
            }
        }
    }

    public static implicit operator Layout(Template template)
    {
        Layout.Builder builder = template.Build().ToBuilder();
        builder.Templates.Add(template);
        return builder.ToLayout();
    }

    public event EventHandler<AttributesUpdatingEventArgs>? AttributesUpdating;

    protected Template(IDictionary attributes)
    {
        switch (attributes)
        {
            case ImmutableDictionary<object, object> dictionary:
                _attributes = dictionary;
                break;

            case IDictionary<object, object> dictionary:
                _attributes = dictionary.ToImmutableDictionary();
                break;

            default:
                _attributes = attributes.Cast<DictionaryEntry>().ToImmutableDictionary(entry => entry.Key, entry => entry.Value)!;
                break;
        }
    }

    protected virtual void OnAttributesUpdating(AttributesUpdatingEventArgs args)
    {
        AttributesUpdating?.Invoke(this, args);
    }

    protected abstract Layout Build();

    private readonly ImmutableDictionary<object, object> _attributes;
}

public class A
{
    private int Num { get; set; }

    public class Holder
    {
        protected A A { get; } = new();
    }

    public static implicit operator IEnumerable(A a)
    {

    }
}

public class B : A.Holder
{
    public class Holder
    {
        protected B B { get; } = new();

        public Holder()
        {
        }
    }
}
