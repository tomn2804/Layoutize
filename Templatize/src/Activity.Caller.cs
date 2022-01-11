using System;

namespace Templatize;

public partial class Activity
{
    public abstract class Caller
    {
        protected void OnProcessed(InvokedEventArgs args)
        {
            foreach (EventHandler<InvokedEventArgs> handler in args.Activity.Invoked)
            {
                handler.Invoke(this, args);
            }
        }

        protected void OnProcessing(InvokingEventArgs args)
        {
            foreach (EventHandler<InvokingEventArgs> handler in args.Activity.Invoking)
            {
                handler.Invoke(this, args);
            }
        }
    }
}
