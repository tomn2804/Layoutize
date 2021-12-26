using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Schemata;

public partial class Connector
{
    public ImmutableQueue<EventHandler<ProcessedEventArgs>> Processed { get; }
    public ImmutableStack<EventHandler<ProcessingEventArgs>> Processing { get; }

    private Connector(ImmutableQueue<EventHandler<ProcessedEventArgs>> processed, ImmutableStack<EventHandler<ProcessingEventArgs>> processing)
    {
        Processed = processed;
        Processing = processing;
    }
}

public partial class Connector
{
    public static readonly Connector Empty = new(ImmutableQueue.Create<EventHandler<ProcessedEventArgs>>(), ImmutableStack.Create<EventHandler<ProcessingEventArgs>>());

    public Builder ToBuilder()
    {
        return new(this);
    }

    public class Builder
    {
        public Queue<EventHandler<ProcessedEventArgs>> Processed { get; }
        public Stack<EventHandler<ProcessingEventArgs>> Processing { get; }

        public Builder()
        {
            Processed = new();
            Processing = new();
        }

        public Builder(Connector connector)
        {
            Processed = new(connector.Processed);
            Processing = new(connector.Processing);
        }

        public Connector ToConnector()
        {
            return new(ImmutableQueue.CreateRange(Processed), ImmutableStack.CreateRange(Processing));
        }
    }
}
