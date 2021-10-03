using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace SchemataPreview
{
    public class FileNameProperty : NameProperty
    {
        public FileNameProperty(ImmutableDefinition definition)
            : base(definition)
        {
        }

        protected override bool TryGetValue([MaybeNullWhen(false)] out string value)
        {
            if (base.TryGetValue(out value))
            {
                if (value.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                {
                    throw new ArgumentException($"Definition '{Key}' property value cannot contains invalid characters. Recieved value: '{Value}'", Key);
                }
                return true;
            }
            return false;
        }
    }
}
