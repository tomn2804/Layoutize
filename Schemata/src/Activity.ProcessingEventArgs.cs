using System;

namespace Schemata;

public sealed partial class Activity
{
    public sealed class ProcessingEventArgs : EventArgs
    {
        public Activity Activity { get; }

        internal ProcessingEventArgs(Activity activity)
        {
            Activity = activity;
        }
    }
}
