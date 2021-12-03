using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Schemata
{
    public abstract class Model
    {
        protected Model(Assembly assembly)
        {
            Assembly = assembly;
        }

        protected Assembly Assembly { get; }
    }

    public class FileModel : Model
    {
        public FileModel(Assembly assembly)
            : base(assembly)
        {
        }
    }

    public abstract class Schema
    {
        protected Schema(IDictionary outline)
        {
            switch (outline)
            {
                case ImmutableDictionary<object, object?> dictionary:
                    Outline = dictionary;
                    break;
                default:
                    Outline = outline.Cast<DictionaryEntry>().ToImmutableDictionary(entry => entry.Key, entry => entry.Value);
                    break;
            }
        }

        public abstract Type ModelType { get; }

        private ImmutableDictionary<object, object?> _outline;

        public ImmutableDictionary<object, object?> Outline
        {
            get => _outline;
            protected set
            {
                _outline = value;
                if (_outline.TryGetKey("Workbench", out object @object) && @object is Workbench workBench)
                {
                    workBench.Build(this);
                }
            }
        }

        public static implicit operator Blueprint(Schema schema)
        {
            return new Blueprint(schema);
        }

        protected abstract Blueprint Build();

        internal Schema Flatten()
        {
            Blueprint blueprint = Build();
            if (blueprint.Schema == this)
            {
                return this;
            }
            if (!ModelType.IsAssignableTo(blueprint.Schema.ModelType))
            {
                throw new InvalidOperationException();
            }
            return blueprint.Schema.Flatten();
        }
    }

    public abstract class Schema<T> : Schema where T : Model
    {
        protected Schema(IDictionary outline)
            : base(outline)
        {
        }

        public override Type ModelType => typeof(T);
    }

    public class FileSchema : Schema<Model>
    {
        public FileSchema(IDictionary outline)
            : base(outline)
        {
        }

        protected override Blueprint Build()
        {
            return new Blueprint(this);
        }
    }

    public class TextFileSchema : Schema<Model>
    {
        public TextFileSchema(IDictionary outline)
            : base(outline)
        {
        }

        protected override Blueprint Build()
        {
            return new FileSchema(Outline);
        }
    }

    public class StrictTextFileSchema : Schema<FileModel>
    {
        public StrictTextFileSchema(IDictionary outline)
            : base(outline)
        {
        }

        protected override Blueprint Build()
        {
            return new TextFileSchema(Outline);
        }
    }

    public class Blueprint
    {
        internal Blueprint(Schema schema)
        {
            Schema = schema;
        }

        protected internal Schema Schema { get; }
    }

    public class Assembly
    {
        internal Assembly(Blueprint blueprint, params KeyValuePair<object, object?>[] args)
        {
            Schema = (Schema)Activator.CreateInstance(
                blueprint.Schema.GetType(),
                blueprint.Schema.Flatten().Outline.SetItems(args)
            )!;
        }

        public Schema Schema { get; }
    }

    public class Workbench
    {
        public Workbench(string path)
        {
            WorkingDirectoryPath = path;
        }

        public string WorkingDirectoryPath { get; }

        public Model Build(Schema schema)
        {
            return Build(new Blueprint(schema));
        }

        private Model Build(Blueprint blueprint)
        {
            Model model = (Model)Activator.CreateInstance(
                blueprint.Schema.ModelType,
                new Assembly(
                    blueprint,
                    KeyValuePair.Create<object, object?>("Workbench", this)
                )
            )!;
            //model.Mount();
            return model;
        }
    }
}
