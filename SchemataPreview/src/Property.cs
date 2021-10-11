﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace SchemataPreview
{
    public abstract class Property<T> : Model.Component
    {
        public abstract string Key { get; }

        public abstract T Value { get; }

        public static implicit operator T(Property<T> @this)
        {
            return @this.Value;
        }

        protected Property(Model model)
            : base(model)
        {
        }

        protected virtual T? GetValue()
        {
            if (Schema.Props.TryGetValue(Key, out object? @object))
            {
                return @object is T result ? result : throw new ArgumentException($"Props property value at key '{Key}' must be of type '{typeof(T)}'. Recieved type: '{@object.GetType()}'.", Key);
            }
            return default;
        }
    }
}
