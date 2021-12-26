using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Schemata;

public abstract partial class Template
{
    public static class RequiredDetails
    {
        public static readonly string Name = "Name";
    }
}

public abstract partial class Template
{
    internal event EventHandler<DetailsUpdatingEventArgs>? DetailsUpdating;

    public IImmutableDictionary<object, object> Details
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

    internal abstract Type ModelType { get; }

    public static implicit operator Blueprint(Template template)
    {
        Blueprint.Builder builder = template.ToBlueprint().ToBuilder();
        if (!template.ModelType.IsAssignableTo(builder.ModelType))
        {
            throw new InvalidOperationException();
        }
        builder.Templates.Add(template);
        return builder.ToBlueprint();
    }

    protected Template(IEnumerable details)
    {
        switch (details)
        {
            case IImmutableDictionary<object, object> dictionary:
                _details = dictionary;
                break;

            case IDictionary<object, object> dictionary:
                _details = dictionary.ToImmutableDictionary();
                break;

            case IDictionary dictionary:
            default:
                _details = details.Cast<DictionaryEntry>().ToImmutableDictionary(entry => entry.Key, entry => entry.Value)!;
                break;
        }
        if (!Details.TryGetValue(RequiredDetails.Name, out object? name))
        {
            throw new ArgumentNullException(RequiredDetails.Name);
        }
    }

    protected virtual void OnDetailsUpdating(DetailsUpdatingEventArgs args)
    {
        DetailsUpdating?.Invoke(this, args);
    }

    protected virtual Blueprint ToBlueprint()
    {
        return new Blueprint.Builder().ToBlueprint();
    }

    private readonly IImmutableDictionary<object, object> _details;
}

public abstract class Template<T> : Template where T : Model
{
    internal override Type ModelType => typeof(T);

    protected Template(IEnumerable details)
        : base(details)
    {
    }
}
