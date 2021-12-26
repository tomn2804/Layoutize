using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Schemata;

public abstract partial class Template
{
    public event EventHandler<DetailsUpdatingEventArgs>? DetailsUpdating;

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
                _details = dictionary.Cast<DictionaryEntry>().ToImmutableDictionary(entry => entry.Key, entry => entry.Value)!;
                break;

            default:
                int key = 0;
                _details = details.Cast<object>().ToImmutableDictionary(_ => (object)key++, value => value);
                break;
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
    public override Type ModelType => typeof(T);

    protected Template(IEnumerable details)
        : base(details)
    {
    }
}

public sealed class BlankTemplate : Template<Model>
{
    public BlankTemplate(IEnumerable details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        return base.ToBlueprint();
    }
}

public sealed class FileTemplate : Template<FileModel>
{
    public FileTemplate(IEnumerable details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        return new BlankTemplate(Details);
    }
}

public sealed class TextFileTemplate : Template<FileModel>
{
    public TextFileTemplate(IEnumerable details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        return new FileTemplate(Details);
    }
}

public sealed class StrictTextFileTemplate : Template<FileModel>
{
    public StrictTextFileTemplate(IEnumerable details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        return new TextFileTemplate(Details);
    }
}
