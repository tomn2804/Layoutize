using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Templata;

public sealed partial class Activity
{
    internal sealed class Builder
    {
        internal Builder()
        {
            Processed = new();
            Processing = new();
        }

        internal Builder(Activity activity)
        {
            Processed = new(activity.Processed);
            Processing = new(activity.Processing);
        }

        internal Queue<EventHandler<ProcessedEventArgs>> Processed { get; }

        internal Stack<EventHandler<ProcessingEventArgs>> Processing { get; }

        internal Activity ToActivity()
        {
            return new() { Processed = Processed.ToImmutableList(), Processing = Processing.ToImmutableList() };
        }
    }
}
