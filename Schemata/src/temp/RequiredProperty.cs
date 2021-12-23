using System;
using System.Diagnostics.CodeAnalysis;

namespace Schemata
{
    public abstract class RequiredProperty<T> : Property<T> where T : notnull
    {
        public override T Value { get; }

        protected RequiredProperty(Model model)
            : base(model)
        {
            Value = GetValue() ?? throw new ArgumentNullException(Key);
        }

        protected RequiredProperty(Model model, T value)
            : base(model)
        {
            Value = value;
        }
    }
}
