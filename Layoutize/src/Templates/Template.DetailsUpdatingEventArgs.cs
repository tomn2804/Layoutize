using System;
using System.Collections.Immutable;

namespace Layoutize.Templates;

public abstract partial class Template
{
    public class AttributesUpdatingEventArgs : EventArgs
    {
        public IImmutableDictionary<object, object> Attributes { get; }

        public AttributesUpdatingEventArgs(IImmutableDictionary<object, object> attributes)
        {
            Attributes = attributes;
        }
    }
}
