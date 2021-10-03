using System;

namespace SchemataPreview
{
    public abstract class RequiredProperty<T> : Property<T> where T : notnull
    {
        public override T Value { get; }

        protected RequiredProperty(ImmutableDefinition definition)
            : base(definition)
        {
            Value = TryGetValue(out T? result) ? result.AssertNotNull() : throw new ArgumentNullException(Key);
        }
    }
}
