using System;
using System.Collections;
using System.Collections.Immutable;
using System.Management.Automation;

namespace SchemataPreview
{
    public abstract class Schema
    {
        public abstract Type ModelType { get; }
        public ImmutableProps Props { get; }

        public abstract Model CreateModel();

        public abstract Model Mount();

        protected Schema(ImmutableProps props)
        {
            Props = props;
        }

        protected abstract Schema Build();
    }

    public abstract class Schema<T> : Schema where T : Model
    {
        public override Type ModelType => typeof(T);

        public override T CreateModel()
        {
            return (T)Activator.CreateInstance(ModelType, this).AssertNotNull();
        }

        public override T Mount()
        {
            Schema schema = Build();
            if (!ModelType.IsAssignableTo(schema.ModelType))
            {
                throw new InvalidOperationException();
            }
            T result = (T)Activator.CreateInstance(ModelType, schema).AssertNotNull();
            //new Pipeline(result).Invoke(PipeOption.Mount);
            return result;
        }

        protected Schema(ImmutableProps props)
            : base(props)
        {
        }
    }
}
