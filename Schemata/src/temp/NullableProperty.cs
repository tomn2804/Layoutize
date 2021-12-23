using System;

namespace Schemata
{
    public abstract class NullableProperty<T> : Property<T?>
    {
        public override T? Value { get; }

        protected NullableProperty(Model model)
            : base(model)
        {
            Value = GetValue();
        }

        protected NullableProperty(Model model, T value)
            : base(model)
        {
            Value = value;
        }
    }
}
