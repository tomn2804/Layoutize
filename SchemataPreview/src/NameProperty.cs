using System;
using System.Diagnostics.CodeAnalysis;

namespace SchemataPreview
{
    public class NameProperty : RequiredProperty<string>
    {
        public NameProperty(ImmutableDefinition definition)
            : base(definition)
        {
        }

        public override string Key => "Name";

        protected override bool TryGetValue([MaybeNullWhen(false)] out string value)
        {
            if (definition.TryGetValue(Key, out object? result) && !string.IsNullOrWhiteSpace(result.ToString()))
            {
                value = result.ToString().AssertNotNull();
                return true;
            }
            value = default;
            return false;
        }
    }
}
