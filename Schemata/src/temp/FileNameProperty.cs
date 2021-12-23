using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Schemata
{
    public class FileNameProperty : NameProperty
    {
        public FileNameProperty(Model model)
            : base(model)
        {
            Validate();
        }

        public FileNameProperty(Model model, string value)
            : base(model, value)
        {
            Validate();
        }

        private void Validate()
        {
            if (Value.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                throw new ArgumentException($"Outline '{Key}' property value cannot contains invalid characters. Recieved value: '{Value}'", Key);
            }
        }
    }
}
