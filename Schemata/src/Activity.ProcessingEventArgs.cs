using System;

namespace Schemata;

public sealed partial class Activity
{
    public sealed class ProcessingEventArgs : EventArgs
    {
        public Model Model { get; }

        internal ProcessingEventArgs(Model model)
        {
            Model = model;
        }
    }
}
