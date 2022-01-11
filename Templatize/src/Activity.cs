using System;
using System.Collections.Generic;

namespace Templatize;

public partial class Activity
{
    internal IEnumerable<EventHandler<InvokedEventArgs>> Invoked { get; init; } = null!;

    internal IEnumerable<EventHandler<InvokingEventArgs>> Invoking { get; init; } = null!;

    public Builder ToBuilder()
    {
        return new(this);
    }

    private Activity()
    {
    }
}
