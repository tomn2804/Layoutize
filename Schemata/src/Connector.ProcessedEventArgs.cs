using System;

namespace Schemata;

public partial class Connector
{
    public class ProcessedEventArgs : EventArgs
    {
        public ProcessedEventArgs(Model model)
        {
            Model = model;
        }

        public Model Model { get; }
    }
}
