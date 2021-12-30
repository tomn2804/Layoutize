using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Schemata;

public abstract partial class Template
{
    internal event EventHandler<DetailsUpdatingEventArgs>? DetailsUpdating;

    protected IImmutableDictionary<object, object> Details
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

    protected abstract Type ModelType { get; }

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

    public virtual Model GetNewModel()
    {
        Model model = (Model)Activator.CreateInstance(ModelType)!;
        return model;
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
    }

    private protected virtual void OnDetailsUpdating(DetailsUpdatingEventArgs args)
    {
        DetailsUpdating?.Invoke(this, args);
    }

    protected virtual Blueprint ToBlueprint()
    {
        if (!Details.TryGetValue(RequiredDetails.Name, out object? name) || string.IsNullOrWhiteSpace((string?)name))
        {
            throw new ArgumentNullException("details", "Details property 'Name' cannot be null, missing, or containing only white spaces.");
        }
        return new Blueprint.Builder().ToBlueprint();
    }

    private readonly IImmutableDictionary<object, object> _details;
}

public abstract partial class Template
{
    public static class RequiredDetails
    {
        public static readonly string Name = "Name";
    }
}

public abstract class Template<T> : Template where T : Model
{
    public override Type ModelType => typeof(T);

    protected Template(IEnumerable details)
        : base(details)
    {
    }
}
