using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Templata;

public abstract partial class Template
{
    public class DetailOption
    {
        public const string Name = nameof(Model.Name);
        public const string Path = nameof(Model.Path);
        public const string Priority = nameof(Model.Priority);

        public const string OnCreating = nameof(OnCreating);
        public const string OnCreated = nameof(OnCreated);

        public const string OnMounting = nameof(OnMounting);
        public const string OnMounted = nameof(OnMounted);
    }
}

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

    internal event EventHandler<DetailsUpdatingEventArgs>? DetailsUpdating;

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

    protected abstract Blueprint ToBlueprint();

    private readonly ImmutableDictionary<object, object> _details;
}

public abstract class Template<T> : Template where T : Model
{
    public override Type ModelType => typeof(T);

    protected Template(IDictionary details)
        : base(details)
    {
    }
}
