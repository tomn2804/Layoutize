using System;
using System.Collections.Generic;

namespace Schemata;

public sealed partial class Activity
{
    public Builder ToBuilder()
    {
        return new(this);
    }

    internal IReadOnlyCollection<EventHandler<ProcessedEventArgs>> Processed { get; }

    internal IReadOnlyCollection<EventHandler<ProcessingEventArgs>> Processing { get; }

    private Activity(IReadOnlyCollection<EventHandler<ProcessedEventArgs>> processed, IReadOnlyCollection<EventHandler<ProcessingEventArgs>> processing)
    {
        Processed = processed;
        Processing = processing;
    }
}
