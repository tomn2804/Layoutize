using System;
using System.Collections.Immutable;

namespace Schemata;

public partial class Blueprint
{
    public abstract partial class Template
    {
        public class DetailsUpdatingEventArgs : EventArgs
        {
            public IImmutableDictionary<object, object?> Details { get; }

            public DetailsUpdatingEventArgs(IImmutableDictionary<object, object?> details)
            {
                Details = details;
            }
        }
    }
}
