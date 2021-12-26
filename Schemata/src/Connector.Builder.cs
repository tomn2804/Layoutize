using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Schemata;

public sealed partial class Connector
{
    public sealed class Builder
    {
        public Builder()
        {
            Processed = new();
            Processing = new();
        }

        internal Builder(Connector connector)
        {
            Processed = new(connector.Processed);
            Processing = new(connector.Processing);
        }

        public Queue<EventHandler<ProcessedEventArgs>> Processed { get; }

        public Stack<EventHandler<ProcessingEventArgs>> Processing { get; }

        public Connector ToConnector()
        {
            return new(ImmutableList.CreateRange(Processed), ImmutableList.CreateRange(Processing));
        }
    }
}
