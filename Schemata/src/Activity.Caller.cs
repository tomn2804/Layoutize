using System;

namespace Schemata;

public sealed partial class Activity
{
    public abstract class Caller
    {
        protected void OnProcessed(ProcessedEventArgs args)
        {
            foreach (EventHandler<ProcessedEventArgs> handler in args.Activity.Processed)
            {
                handler.Invoke(this, args);
            }
        }

        protected void OnProcessing(ProcessingEventArgs args)
        {
            foreach (EventHandler<ProcessingEventArgs> handler in args.Activity.Processing)
            {
                handler.Invoke(this, args);
            }
        }
    }
}
