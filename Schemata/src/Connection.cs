using System;
using System.Collections.Generic;
using System.Linq;

namespace Schemata;

public partial class Connector
{
    public class ProcessingEventArgs : EventArgs
    {
        public Model Model { get; }

        public ProcessingEventArgs(Model model)
        {
            Model = model;
        }
    }

    public class ProcessedEventArgs : EventArgs
    {
        public Model Model { get; }

        public ProcessedEventArgs(Model model)
        {
            Model = model;
        }
    }
}

public partial class Connection
{
    private Stack<EventHandler<Connector.ProcessedEventArgs>?> Callbacks { get; } = new();

    public Model Model { get; }

    public Connection(Model model)
    {
        Model = model;
    }

    public void Push(Connector connection)
    {
        connection.OnProcessing(this, new(Model));
        Callbacks.Push(connection.OnProcessed);
    }
}

public partial class Connection : IDisposable
{
    public virtual void Dispose()
    {
        while (Callbacks.Any())
        {
            Callbacks.Pop()?.Invoke(this, new(Model));
        }
        GC.SuppressFinalize(this);
    }
}
