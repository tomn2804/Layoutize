using System;
using System.Diagnostics.CodeAnalysis;

namespace Schemata
{
    public class NameProperty : RequiredProperty<string>
    {
        public NameProperty(Model model)
            : base(model)
        {
        }

        public NameProperty(Model model, string value)
            : base(model, value)
        {
        }

        public override string Key => "Name";

        protected override string? GetValue()
        {
            if (Schema.Outline.TryGetValue(Key, out object? @object) && @object.ToString() is string result && !string.IsNullOrWhiteSpace(result))
            {
                return result;
            }
            return default;
        }
    }
}
