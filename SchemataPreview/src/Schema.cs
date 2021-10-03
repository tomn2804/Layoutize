using System;
using System.Collections;
using System.Collections.Immutable;
using System.Management.Automation;

namespace SchemataPreview
{
    public abstract class Schema
    {
        public ImmutableDefinition Definition { get; }

        public abstract Model Mount();

        protected Schema(Definition definition)
        {
            Definition = definition.ToImmutable();
        }

        protected abstract Schema Build();

        protected abstract Type ModelType { get; }
    }

    public abstract class Schema<T> : Schema where T : Model
    {
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

        protected Schema(Definition definition)
            : base(definition)
        {
        }

        protected override Type ModelType => typeof(T);
    }
}
