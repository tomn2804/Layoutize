using System;
using System.Collections.Immutable;

namespace Templatize;

public abstract partial class Template
{
    public class DetailsUpdatingEventArgs : EventArgs
    {
        public IImmutableDictionary<object, object> Details { get; }

        public DetailsUpdatingEventArgs(IImmutableDictionary<object, object> details)
        {
            Details = details;
        }
    }
}
