using System;
using System.Collections.Generic;
using System.Linq;

namespace Schemata;

public partial class Connection
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

    public partial class Segment
    {
        private Stack<EventHandler<ProcessedEventArgs>?> Callbacks { get; } = new();

        public Model Model { get; }

        public Segment(Model model)
        {
            Model = model;
        }

        public void Push(Connection connection)
        {
            connection.Processing?.Invoke(this, new(Model));
            Callbacks.Push(connection.Processed);
        }
    }

    public partial class Segment : IDisposable
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
}
