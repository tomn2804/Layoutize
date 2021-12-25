using System;
using System.Collections.Immutable;

namespace Schemata;

public abstract partial class Template
{
    public class DetailsUpdatingEventArgs : EventArgs
    {
        public DetailsUpdatingEventArgs(IImmutableDictionary<object, object> details)
        {
            Details = details;
        }

        public IImmutableDictionary<object, object> Details { get; }
    }
}
