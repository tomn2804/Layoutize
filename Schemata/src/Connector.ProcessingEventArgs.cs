using System;

namespace Schemata;

public sealed partial class Connector
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
