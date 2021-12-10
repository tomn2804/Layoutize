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

    public Blueprint Blueprint
    {
        get => _blueprint;
        set
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

public abstract class Template
{
    protected Template(IEnumerable variables)
    {
        switch (variables)
        {
            case IImmutableDictionary<object, object?> dictionary:
                Variables = dictionary;
                break;
            case IDictionary<object, object?> dictionary:
                Variables = dictionary.ToImmutableDictionary();
                break;
            case IDictionary dictionary:
                Variables = dictionary.Cast<DictionaryEntry>().ToImmutableDictionary(entry => entry.Key, entry => entry.Value);
                break;
            default:
                int key = 0;
                Variables = variables.Cast<object?>().ToImmutableDictionary(_ => (object)key++, value => value);
                break;
        }
    }

    public abstract Type ModelType { get; }

    public IImmutableDictionary<object, object?> Variables { get; }

    protected abstract Blueprint Inflate();

    protected Blueprint InflateTo(Template template)
    {
        if (!ModelType.IsAssignableTo(template.ModelType))
        {
            throw new InvalidOperationException();
        }
        return template.Inflate();
    }
}

public sealed partial class Blueprint
{
    private Blueprint(BlankTemplate template)
    {
        Variables = template.Variables;
        ModelType = template.ModelType;
    }

    public IImmutableDictionary<object, object?> Variables { get; }
    public Type ModelType { get; }
}

public sealed partial class Blueprint
{
    public class BlankTemplate : Template<Model>
    {
        public BlankTemplate(IEnumerable variables)
            : base(variables)
        {
        }

        protected override Blueprint Inflate()
        {
            Console.WriteLine(nameof(BlankTemplate));
            return new(this);
        }
    }
}

public abstract class Template<T> : Template where T : Model
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

    protected override Blueprint Inflate()
    {
        Console.WriteLine(nameof(FileTemplate));
        return InflateTo(new Blueprint.BlankTemplate(Variables));
    }
}

public class TextFileTemplate : Template<FileModel>
{
    public TextFileTemplate(IEnumerable variables)
        : base(variables)
    {
    }

    protected override Blueprint Inflate()
    {
        Console.WriteLine(nameof(TextFileTemplate));
        return InflateTo(new FileTemplate(Variables));
    }
}

public class StrictTextFileTemplate : Template<FileModel>
{
    public StrictTextFileTemplate(IEnumerable variables)
        : base(variables)
    {
    }

    protected override Blueprint Inflate()
    {
        Console.WriteLine(nameof(StrictTextFileTemplate));
        return InflateTo(new TextFileTemplate(Variables));
    }
}

//public class Workbench
//{
//    public Workbench(string path)
//    {
//        WorkingDirectoryPath = path;
//    }

//    public string WorkingDirectoryPath { get; }

//    public Model Build(Template template, params KeyValuePair<object, object?>[] variables)
//    {
//        if (variables.Any())
//        {
//            template = (Template)Activator.CreateInstance(template.GetType(), template.Variables.SetItems(variables))!;
//        }
//        return Build(template);
//    }

//    public Model Build(Blueprint blueprint)
//    {
//        Model model = (Model)Activator.CreateInstance(blueprint.ModelType, blueprint)!;
//        //model.Mount();
//        return model;
//    }
//}
