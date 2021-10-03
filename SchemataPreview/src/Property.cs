using System;
using System.Diagnostics.CodeAnalysis;

namespace SchemataPreview
{
    public abstract class Property<T>
    {
        public abstract string Key { get; }
        public abstract T Value { get; }

        public static implicit operator T(Property<T> @this)
        {
            return @this.Value;
        }

        protected Property(ImmutableDefinition definition)
        {
            definition = definition;
        }

        protected ImmutableDefinition definition { get; }

        protected virtual bool TryGetValue([MaybeNullWhen(false)] out T value)
        {
            if (definition.TryGetValue(Key, out object? @object))
            {
                value = @object is T result ? result : throw new ArgumentException($"Definition property value at key {Key} must be of type '{typeof(T)}'. Recieved type: '{@object.GetType()}'.", Key);
                return true;
            }
            value = default;
            return false;
        }
    }
}
