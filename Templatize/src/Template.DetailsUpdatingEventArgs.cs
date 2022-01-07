using System;
using System.Collections.Immutable;

namespace Templata;

public abstract partial class Template
{
    public sealed class DetailsUpdatingEventArgs : EventArgs
    {
        public IImmutableDictionary<object, object> Details { get; }

        internal DetailsUpdatingEventArgs(IImmutableDictionary<object, object> details)
        {
            Details = details;
        }
    }
}
