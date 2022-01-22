using System;

namespace Templatize.Views;

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
