using System;
using System.Collections;
using System.Collections.Immutable;
using System.Management.Automation;

namespace Schemata
{
    public abstract class Schema
    {
        public abstract Type ModelType { get; }
        public ImmutableOutline Outline { get; }

        public abstract Model GetNewModel();

        public abstract Model Mount();

        protected Schema(ImmutableOutline outline)
        {
            Outline = outline;
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

        protected Schema(ImmutableOutline outline)
            : base(outline)
        {
        }
    }
}
