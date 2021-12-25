using System;
using System.Collections.Generic;

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

public partial class Connector
{
    public Stack<EventHandler<ProcessingEventArgs>> Processing { get; } = new();

    public Queue<EventHandler<ProcessedEventArgs>> Processed { get; } = new();
}

public partial class Connector
{
    public class Owner
    {
        protected Owner()
        {
        }

        protected void OnProcessing(Connector connector, ProcessingEventArgs args)
        {
            foreach (EventHandler<ProcessingEventArgs> handler in connector.Processing)
            {
                handler.Invoke(this, args);
            }
        }

        protected void OnProcessed(Connector connector, ProcessedEventArgs args)
        {
            foreach (EventHandler<ProcessedEventArgs> handler in connector.Processed)
            {
                handler.Invoke(this, args);
            }
        }
    }
}
