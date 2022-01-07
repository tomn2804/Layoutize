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
        public const string Name = nameof(View.Name);
        public const string Path = nameof(View.Path);
        public const string Priority = nameof(View.Priority);

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

    public abstract Type ViewType { get; }

    public static implicit operator Context(Template template)
    {
        Context.Builder builder = template.ToBlueprint().ToBuilder();
        if (!template.ViewType.IsAssignableTo(builder.ViewType))
        {
            throw new InvalidOperationException($"Template '{builder.Templates.Last().GetType().FullName}' requires a view that derives from '{builder.ViewType.FullName}'.");
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

    protected abstract Context ToBlueprint();

    private readonly ImmutableDictionary<object, object> _details;
}

public abstract class Template<T> : Template where T : View
{
    public override Type ViewType => typeof(T);

    protected Template(IDictionary details)
        : base(details)
    {
    }
}
