using System;
using System.Collections.Generic;

namespace Templata;

public sealed partial class Activity
{
    internal IEnumerable<EventHandler<ProcessedEventArgs>> Processed { get; init; } = null!;

    internal IEnumerable<EventHandler<ProcessingEventArgs>> Processing { get; init; } = null!;

    internal Builder ToBuilder()
    {
        return new(this);
    }

    private Activity()
    {
    }
}
