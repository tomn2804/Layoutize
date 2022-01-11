using System;

namespace Templatize;

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
