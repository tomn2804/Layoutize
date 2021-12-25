using System;
using System.Collections.Generic;

namespace Schemata;

public partial class Connector
{
    public Queue<EventHandler<ProcessedEventArgs>> Processed { get; } = new();
    public Stack<EventHandler<ProcessingEventArgs>> Processing { get; } = new();
}
