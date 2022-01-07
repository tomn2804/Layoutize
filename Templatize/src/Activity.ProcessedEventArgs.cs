using System;

namespace Templata;

public sealed partial class Activity
{
    public sealed class ProcessedEventArgs : EventArgs
    {
        public Activity Activity { get; }

        internal ProcessedEventArgs(Activity activity)
        {
            Activity = activity;
        }
    }
}
