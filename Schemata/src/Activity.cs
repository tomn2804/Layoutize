using System;
using System.Collections.Generic;

namespace Schemata;

public sealed partial class Activity
{
    internal Builder ToBuilder()
    {
        return new(this);
    }

    internal IEnumerable<EventHandler<ProcessedEventArgs>> Processed { get; init; } = null!;

    internal IEnumerable<EventHandler<ProcessingEventArgs>> Processing { get; init; } = null!;

    private Activity()
    {
    }
}
