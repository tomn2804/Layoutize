using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Schemata
{
    public abstract class Model
    {
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
                // Mount()
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

    public class Blueprint
    {
        public Blueprint(IImmutableDictionary<object, object?> variables)
        {
            Variables = variables;
        }

        public IImmutableDictionary<object, object?> Variables { get; }
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
            Action<Model> handleOnMounting = (model) =>
            {
                if (!model.GetType().IsAssignableTo(ModelType))
                {
                    throw new InvalidOperationException();
                }
                if (Variables.TryGetValue("OnMounting", out object? @object) && @object is Action<Model> onMounting)
                {
                    onMounting(model);
                }
            };
            Variables = Variables.SetItem("OnMounting", handleOnMounting);
        }

        public abstract Type ModelType { get; }
        public IImmutableDictionary<object, object?> Variables { get; }

        public static implicit operator Blueprint(Template template)
        {
            return template.Build();
        }

        protected abstract Blueprint Build();
    }

    public abstract class Template<T> : Template where T : Model
    {
        protected Template(IEnumerable variables)
            : base(variables)
        {
        }

        public override Type ModelType => typeof(T);
    }

    public class FileTemplate : Template<Model>
    {
        public FileTemplate(IEnumerable variables)
            : base(variables)
        {
        }

        protected override Blueprint Build()
        {
            Console.WriteLine(nameof(FileTemplate));
            return new Blueprint(Variables);
        }
    }

    public class TextFileTemplate : Template<Model>
    {
        public TextFileTemplate(IEnumerable variables)
            : base(variables)
        {
        }

        protected override Blueprint Build()
        {
            Console.WriteLine(nameof(TextFileTemplate));
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
            Console.WriteLine(nameof(StrictTextFileTemplate));
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

        public Model Build(Template template)
        {
            Model model = (Model)Activator.CreateInstance(template.ModelType, (Blueprint)template)!;
            //model.Mount();
            return model;
        }
    }
}
