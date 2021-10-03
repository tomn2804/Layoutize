using System;

namespace SchemataPreview
{
    public abstract class DefaultProperty<T> : Property<T> where T : notnull
    {
        public override T Value { get; }

        protected DefaultProperty(ImmutableDefinition definition)
            : base(definition)
        {
            Value = TryGetValue(out T? result) ? result.AssertNotNull() : DefaultValue;
        }

        protected abstract T DefaultValue { get; }
    }
}
