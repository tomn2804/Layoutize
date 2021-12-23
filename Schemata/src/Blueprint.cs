using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Schemata;

public abstract partial class Model : Blueprint.Owner
{
    protected Model(Blueprint blueprint)
        : base(blueprint)
    {
        Debug.Assert(Blueprint.ModelType == GetType());
    }
}

public abstract partial class Model
{
    public class Workbench
    {
        public Workbench(string path)
        {
            WorkingDirectoryPath = path;
        }

        public string WorkingDirectoryPath { get; }
    }
}

public class FileModel : Model
{
    public FileModel(Blueprint blueprint)
        : base(blueprint)
    {
    }
}

public class DetailsUpdatingEventArgs : EventArgs
{
    public IImmutableDictionary<object, object?> NewDetails { get; }

    public DetailsUpdatingEventArgs(IImmutableDictionary<object, object?> newDetails)
    {
        NewDetails = newDetails;
    }
}

public partial class Blueprint
{
    private List<Template> Templates { get; } = new();

    public Type ModelType => Templates.LastOrDefault()?.ModelType ?? typeof(Model);

    private Blueprint()
    {
    }
}

public partial class Blueprint
{
    public abstract class Owner : IDisposable
    {
        protected Blueprint Blueprint { get; private set; }

        protected Owner(Blueprint blueprint)
        {
            Blueprint = blueprint;
            foreach (Template template in Blueprint.Templates)
            {
                template.DetailsUpdating += UpdateBlueprint;
            }
        }

        protected virtual void UpdateBlueprint(object? sender, DetailsUpdatingEventArgs args)
        {
            Blueprint newBlueprint = (Template)Activator.CreateInstance(sender!.GetType(), args.NewDetails)!;

            int senderIndex = Blueprint.Templates.IndexOf((Template)sender);
            Debug.Assert(newBlueprint.Templates.Count - 1 == senderIndex);

            for (int i = 0; i <= senderIndex; ++i)
            {
                Template oldTemplate = Blueprint.Templates[i];
                Template newTemplate = newBlueprint.Templates[i];

                Debug.Assert(oldTemplate != newTemplate);

                oldTemplate.DetailsUpdating -= UpdateBlueprint;
                newTemplate.DetailsUpdating += UpdateBlueprint;
            }

            for (int i = senderIndex + 1; i < Blueprint.Templates.Count; ++i)
            {
                newBlueprint.Templates.Add(Blueprint.Templates[i]);
            }

            Blueprint = newBlueprint;
        }

        public void Dispose()
        {
            foreach (Template template in Blueprint.Templates)
            {
                template.DetailsUpdating -= UpdateBlueprint;
            }
        }
    }
}

public partial class Blueprint
{
    public abstract class Template
    {
        protected Template(IEnumerable details)
        {
            switch (details)
            {
                case IImmutableDictionary<object, object?> dictionary:
                    _details = dictionary;
                    break;

                case IDictionary<object, object?> dictionary:
                    _details = dictionary.ToImmutableDictionary();
                    break;

                case IDictionary dictionary:
                    _details = dictionary.Cast<DictionaryEntry>().ToImmutableDictionary(entry => entry.Key, entry => entry.Value);
                    break;

                default:
                    int key = 0;
                    _details = details.Cast<object?>().ToImmutableDictionary(_ => (object)key++, value => value);
                    break;
            }
        }

        public static implicit operator Blueprint(Template template)
        {
            Blueprint blueprint = template.ToBlueprint();
            if (!template.ModelType.IsAssignableTo(blueprint.ModelType))
            {
                throw new InvalidOperationException();
            }
            blueprint.Templates.Add(template);
            return blueprint;
        }

        public event EventHandler<DetailsUpdatingEventArgs>? DetailsUpdating;

        private readonly IImmutableDictionary<object, object?> _details;

        public IImmutableDictionary<object, object?> Details
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

        protected virtual void OnDetailsUpdating(DetailsUpdatingEventArgs args)
        {
            DetailsUpdating?.Invoke(this, args);
        }

        public abstract Type ModelType { get; }

        protected virtual Blueprint ToBlueprint()
        {
            return new();
        }
    }
}

public abstract class Template<T> : Blueprint.Template where T : Model
{
    protected Template(IEnumerable details)
        : base(details)
    {
    }

    public override Type ModelType => typeof(T);
}

public class BlankTemplate : Template<Model>
{
    public BlankTemplate(IEnumerable details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        Console.WriteLine(nameof(BlankTemplate));
        return base.ToBlueprint();
    }
}

public class FileTemplate : Template<FileModel>
{
    public FileTemplate(IEnumerable details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        Console.WriteLine(nameof(FileTemplate));
        return new BlankTemplate(Details);
    }
}

public class TextFileTemplate : Template<FileModel>
{
    public TextFileTemplate(IEnumerable details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        Console.WriteLine(nameof(TextFileTemplate));
        return new FileTemplate(Details);
    }
}

public class StrictTextFileTemplate : Template<FileModel>
{
    public StrictTextFileTemplate(IEnumerable details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        Console.WriteLine(nameof(StrictTextFileTemplate));
        return new TextFileTemplate(Details);
    }
}
