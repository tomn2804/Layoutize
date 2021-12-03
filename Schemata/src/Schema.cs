using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Schemata
{
    public class Model
    {
        public Model(Assembly assembly)
        {
            Assembly = assembly;
        }

        public Assembly Assembly { get; }
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
        public ImmutableDictionary<object, object?> Outline { get; }

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
        public Schema(IDictionary outline)
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

        internal Schema Schema { get; }
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
                    KeyValuePair.Create<object, object?>("Path", WorkingDirectoryPath)
                )
            )!;
            //model.Mount();
            return model;
        }
    }
}
