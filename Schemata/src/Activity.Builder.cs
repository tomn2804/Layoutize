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

        public Queue<EventHandler<ProcessedEventArgs>> Processed { get; }

        public Stack<EventHandler<ProcessingEventArgs>> Processing { get; }

        public Activity ToConnector()
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
