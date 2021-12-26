using System;

namespace Schemata;

public sealed partial class Connector
{
    public abstract class Owner
    {
        protected void OnProcessed(Connector connector, ProcessedEventArgs args)
        {
            foreach (EventHandler<ProcessedEventArgs> handler in connector.Processed)
            {
                handler.Invoke(this, args);
            }
        }

        protected void OnProcessing(Connector connector, ProcessingEventArgs args)
        {
            foreach (EventHandler<ProcessingEventArgs> handler in connector.Processing)
            {
                handler.Invoke(this, args);
            }
        }
    }
}
