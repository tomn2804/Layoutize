using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Schemata;

public sealed partial class Activity
{
    public sealed class Builder
    {
        public Builder()
        {
            Processed = new();
            Processing = new();
        }

        public Stack<EventHandler<ProcessedEventArgs>> Processed { get; }

        public Queue<EventHandler<ProcessingEventArgs>> Processing { get; }

        public Activity ToActivity()
        {
            return new(ImmutableList.CreateRange(Processed), ImmutableList.CreateRange(Processing));
        }

        internal Builder(Activity activity)
        {
            Processed = new(activity.Processed);
            Processing = new(activity.Processing);
        }
    }
}
