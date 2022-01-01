using System;

namespace Schemata;

public sealed partial class Activity
{
    public abstract class Owner
    {
        protected void OnProcessed(Activity activity, ProcessedEventArgs args)
        {
            foreach (EventHandler<ProcessedEventArgs> handler in activity.Processed)
            {
                handler.Invoke(this, args);
            }
        }

        protected void OnProcessing(Activity activity, ProcessingEventArgs args)
        {
            foreach (EventHandler<ProcessingEventArgs> handler in activity.Processing)
            {
                handler.Invoke(this, args);
            }
        }
    }
}
