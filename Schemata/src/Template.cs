using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Schemata
{
    public abstract class Model
    {
        internal enum TemplateArgs
        {
            OnMounting
        }

        protected Model(Blueprint blueprint)
        {
            Blueprint = blueprint;
            Blueprint.Template.VariablesUpdateEvent += HandleVariablesUpdateEvent;
        }

        protected virtual void HandleVariablesUpdateEvent(object? template, VariablesUpdateEventArgs args)
        {
            Blueprint = args.Template;
            // Remount
        }

        protected Blueprint Blueprint { get; set; }
    }

    public class FileModel : Model
    {
        public FileModel(Blueprint blueprint)
            : base(blueprint)
        {
        }
    }

    public class VariablesUpdateEventArgs : EventArgs
    {
        public VariablesUpdateEventArgs(Template template)
        {
            Template = template;
        }

        public Template Template { get; }
    }

    public class Blueprint
    {
        private Blueprint(Template template)
        {
            Debug.Assert(template is BlankTemplate);
            Template = template;
        }

        public Template Template { get; }

        public class BlankTemplate : Template<Model>
        {
            public BlankTemplate(IEnumerable variables)
                : base(variables)
            {
            }

            protected override Blueprint Build()
            {
                Console.WriteLine(nameof(BlankTemplate));
                return new Blueprint(this);
            }
        }
    }

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
            Action<Model> handleOnMounting = (model) =>
            {
                if (!model.GetType().IsAssignableTo(ModelType))
                {
                    throw new InvalidOperationException();
                }
                if (Variables.TryGetValue(Model.TemplateArgs.OnMounting, out object? @object) && @object is Action<Model> onMounting)
                {
                    onMounting(model);
                }
            };
            _variables = Variables.SetItem(Model.TemplateArgs.OnMounting, handleOnMounting);
        }

        public abstract Type ModelType { get; }

        private readonly IImmutableDictionary<object, object?> _variables;

        public IImmutableDictionary<object, object?> Variables
        {
            get => _variables;
            set
            {
                if (value != _variables)
                {
                    RaiseVariablesUpdateEvent(value);
                }
            }
        }

        public event EventHandler<VariablesUpdateEventArgs>? VariablesUpdateEvent;

        private void RaiseVariablesUpdateEvent(IImmutableDictionary<object, object?> variables)
        {
            VariablesUpdateEvent?.Invoke(this, new VariablesUpdateEventArgs((Template)Activator.CreateInstance(GetType(), variables)!));
        }

        public static implicit operator Blueprint(Template template)
        {
            return template.Build();
        }

        protected abstract Blueprint Build();
    }

    public abstract class Template<T> : Template where T : Model
    {
        public Template(IEnumerable variables)
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
            return new Blueprint.BlankTemplate(Variables);
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
