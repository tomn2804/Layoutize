using System;

namespace SchemataPreview
{
    public abstract class NullableProperty<T> : Property<T?>
    {
        public override T? Value { get; }

        protected NullableProperty(ImmutableDefinition definition)
            : base(definition)
        {
            Value = TryGetValue(out T? result) ? result.AssertNotNull() : default;
        }
    }
}
