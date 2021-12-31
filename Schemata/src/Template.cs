using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace Schemata;

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

    public abstract Type ModelType { get; }

    public static implicit operator Blueprint(Template template)
    {
        Blueprint.Builder builder = template.ToBlueprint().ToBuilder();
        if (!template.ModelType.IsAssignableTo(builder.ModelType))
        {
            throw new InvalidOperationException($"Template '{builder.Templates.Last().GetType().FullName}' requires a model that derives from '{builder.ModelType.FullName}'.");
        }
        builder.Templates.Add(template);
        return builder.ToBlueprint();
    }

    protected Template(IDictionary details)
    {
        switch (details)
        {
            case IImmutableDictionary<object, object> dictionary:
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

    private protected virtual void OnDetailsUpdating(DetailsUpdatingEventArgs args)
    {
        DetailsUpdating?.Invoke(this, args);
    }

    protected virtual Blueprint ToBlueprint()
    {
        string? name = Details[RequiredDetails.Name]?.ToString();
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException("details", $"Details value property '{RequiredDetails.Name}' cannot be null or containing only white spaces.");
        }
        return new Blueprint.Builder(name).ToBlueprint();
    }

    private readonly IImmutableDictionary<object, object> _details;
}

public abstract partial class Template
{
    public static class RequiredDetails
    {
        public static readonly string Name = "Name";
    }

    public static class OptionalDetails
    {
        public static readonly string OnMounting = "OnMounting";
        public static readonly string OnMounted = "OnMounted";
    }
}

public abstract class Template<T> : Template where T : Model
{
    public override Type ModelType => typeof(T);

    protected Template(IDictionary details)
        : base(details)
    {
    }
}
