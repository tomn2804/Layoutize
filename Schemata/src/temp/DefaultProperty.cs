using System;
using System.Diagnostics.CodeAnalysis;

namespace Schemata
{
    public abstract class DefaultProperty<T> : Property<T> where T : notnull
    {
        public override T Value { get; }

        protected DefaultProperty(Model model)
            : base(model)
        {
            Value = GetValue() ?? GetDefaultValue();
        }

        protected DefaultProperty(Model model, T value)
            : base(model)
        {
            Value = value;
        }

        protected abstract T GetDefaultValue();
    }
}
