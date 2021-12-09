using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Schemata;

public abstract partial class Model
{
    public enum TemplateVariables
    {
        OnInitializing
    }

    protected Model(Blueprint blueprint)
    {
        _blueprint = blueprint;
    }

    private Blueprint _blueprint;

    protected Blueprint Blueprint
    {
        get => _blueprint;
        private set
        {
            _blueprint = value;
            // Remount
        }
    }
}

public class FileModel : Model
{
    public FileModel(Blueprint blueprint)
        : base(blueprint)
    {
    }
}

public sealed partial class Blueprint
{
    private Blueprint(BlankTemplate template)
    {
        Template = template;
    }

    private BlankTemplate Template { get; }

    public IImmutableDictionary<object, object?> Variables => Template.Variables;
    public Type ModelType => Template.ModelType;
}

public sealed partial class Blueprint
{
    public class BlankTemplate : Template<Model>
    {
        public BlankTemplate(IEnumerable variables)
            : base(variables)
        {
        }

        protected override Blueprint Build()
        {
            Debug.WriteLine(nameof(BlankTemplate));
            return new(this);
        }
    }
}

public class VariablesUpdatingEventArgs : EventArgs
{
    public VariablesUpdatingEventArgs(IImmutableDictionary<object, object?> variables)
    {
        Variables = variables;
    }

    public IImmutableDictionary<object, object?> Variables { get; }
}

public abstract partial class Model
{
    public abstract class Template
    {
        protected Template(IEnumerable variables)
        {
            switch (variables)
            {
                case IImmutableDictionary<object, object?> dictionary:
                    _variables = dictionary;
                    break;
                case IDictionary<object, object?> dictionary:
                    _variables = dictionary.ToImmutableDictionary();
                    break;
                case IDictionary dictionary:
                    _variables = dictionary.Cast<DictionaryEntry>().ToImmutableDictionary(entry => entry.Key, entry => entry.Value);
                    break;
                default:
                    int key = 0;
                    _variables = variables.Cast<object?>().ToImmutableDictionary(_ => (object)key++, value => value);
                    break;
            }
            Func<Model, bool> handleOnInitializing = (model) =>
            {
                if (!model.GetType().IsAssignableTo(ModelType))
                {
                    throw new InvalidOperationException();
                }
                void updateModel(object? sender, VariablesUpdatingEventArgs e)
                {
                    model.Blueprint = (Blueprint)Activator.CreateInstance(sender!.GetType(), e.Variables)!;
                    VariablesUpdating -= updateModel;
                }
                VariablesUpdating += updateModel;
                return ((Func<Model, bool>)Variables[TemplateVariables.OnInitializing]!)(model);
            };
            _variables = Variables.SetItem(TemplateVariables.OnInitializing, handleOnInitializing);
        }

        public abstract Type ModelType { get; }

        private readonly IImmutableDictionary<object, object?> _variables;

        public IImmutableDictionary<object, object?> Variables
        {
            get => _variables;
            protected set
            {
                if (value != _variables)
                {
                    OnVariablesUpdating(new VariablesUpdatingEventArgs(value));
                }
            }
        }

        public event EventHandler<VariablesUpdatingEventArgs>? VariablesUpdating;

        protected virtual void OnVariablesUpdating(VariablesUpdatingEventArgs e)
        {
            VariablesUpdating?.Invoke(this, e);
        }

        public static implicit operator Blueprint(Template template)
        {
            return template.Build();
        }

        protected abstract Blueprint Build();
    }
}

public abstract class Template<T> : Model.Template where T : Model
{
    protected Template(IEnumerable variables)
        : base(variables)
    {
    }

    public override Type ModelType => typeof(T);
}

public class FileTemplate : Template<FileModel>
{
    public FileTemplate(IEnumerable variables)
        : base(variables)
    {
    }

    protected override Blueprint Build()
    {
        Debug.WriteLine(nameof(FileTemplate));
        return new Blueprint.BlankTemplate(Variables);
    }
}

public class TextFileTemplate : Template<FileModel>
{
    public TextFileTemplate(IEnumerable variables)
        : base(variables)
    {
    }

    protected override Blueprint Build()
    {
        Debug.WriteLine(nameof(TextFileTemplate));
        return new FileTemplate(Variables);
    }
}

public class StrictTextFileTemplate : Template<FileModel>
{
    public StrictTextFileTemplate(IEnumerable variables)
        : base(variables)
    {
    }

    protected override Blueprint Build()
    {
        Debug.WriteLine(nameof(StrictTextFileTemplate));
        return new TextFileTemplate(Variables);
    }
}

public class Workbench
{
    public Workbench(string path)
    {
        WorkingDirectoryPath = path;
    }

    public string WorkingDirectoryPath { get; }

    public Model Build(Model.Template template, params KeyValuePair<object, object?>[] variables)
    {
        if (variables.Any())
        {
            template = (Model.Template)Activator.CreateInstance(template.GetType(), template.Variables.SetItems(variables))!;
        }
        return Build(template);
    }

    public Model Build(Blueprint blueprint)
    {
        Model model = (Model)Activator.CreateInstance(blueprint.ModelType, blueprint)!;
        //model.Mount();
        return model;
    }
}
