using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Schemata;

public sealed partial class Connector
{
    public Builder ToBuilder()
    {
        return new(this);
    }

    internal IReadOnlyCollection<EventHandler<ProcessedEventArgs>> Processed { get; }

    internal IReadOnlyCollection<EventHandler<ProcessingEventArgs>> Processing { get; }

    private Connector(IReadOnlyCollection<EventHandler<ProcessedEventArgs>> processed, IReadOnlyCollection<EventHandler<ProcessingEventArgs>> processing)
    {
        Processed = processed;
        Processing = processing;
    }
}
