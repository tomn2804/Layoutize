using System;

namespace Templatize;

public partial class Activity
{
    public class InvokedEventArgs : EventArgs
    {
        public Activity Activity { get; }

        public InvokedEventArgs(Activity activity)
        {
            Activity = activity;
        }
    }
}
