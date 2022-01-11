using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Templatize;

public partial class Activity
{
    public sealed class Builder
    {
        public Builder()
        {
            Invoked = new();
            Invoking = new();
        }

        public Builder(Activity activity)
        {
            Invoked = new(activity.Invoked);
            Invoking = new(activity.Invoking);
        }

        public Queue<EventHandler<InvokedEventArgs>> Invoked { get; }

        public Stack<EventHandler<InvokingEventArgs>> Invoking { get; }

        public Activity ToActivity()
        {
            return new() { Invoked = Invoked.ToImmutableList(), Invoking = Invoking.ToImmutableList() };
        }
    }
}
