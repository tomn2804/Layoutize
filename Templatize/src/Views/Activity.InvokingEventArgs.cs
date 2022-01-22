using System;

namespace Templatize.Views;

public partial class Activity
{
    public class InvokingEventArgs : EventArgs
    {
        public Activity Activity { get; }

        public InvokingEventArgs(Activity activity)
        {
            Activity = activity;
        }
    }
}
