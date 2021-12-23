using System;
using System.Collections.Immutable;

namespace Schemata;

public class DetailsUpdatingEventArgs : EventArgs
{
    public IImmutableDictionary<object, object?> NewDetails { get; }

    public DetailsUpdatingEventArgs(IImmutableDictionary<object, object?> newDetails)
    {
        NewDetails = newDetails;
    }
}
