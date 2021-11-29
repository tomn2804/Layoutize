using System;
using System.Collections;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Linq;

namespace Schemata
{
    public abstract class Schema
    {
        public abstract Type ModelType { get; }
        public ImmutableDictionary<object, object?> Outline { get; }

        public abstract Model GetNewModel();

        public abstract Model Mount();

        protected Schema(IDictionary outline)
        {
            Outline = outline.Cast<DictionaryEntry>().ToImmutableDictionary(entry => entry.Key, entry => entry.Value);
        }

        protected abstract Schema Build();
    }

    public abstract class Schema<T> : Schema where T : Model
    {
        public override Type ModelType => typeof(T);

        public override T GetNewModel()
        {
            Schema schema = Build();
            if (!ModelType.IsAssignableTo(schema.ModelType))
            {
                throw new InvalidOperationException();
            }
            return (T)Activator.CreateInstance(ModelType, schema).AssertNotNull();
        }

        public override T Mount()
        {
            T model = GetNewModel();
            model.GetNewPipeline().Invoke(PipeOption.Mount);
            return model;
        }

        protected Schema(IDictionary outline)
            : base(outline)
        {
        }
    }
}
