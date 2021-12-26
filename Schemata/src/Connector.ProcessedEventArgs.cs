using System;

namespace Schemata;

public sealed partial class Connector
{
    public sealed class ProcessedEventArgs : EventArgs
    {
        public Model Model { get; }

        internal ProcessedEventArgs(Model model)
        {
            Model = model;
        }
    }
}
