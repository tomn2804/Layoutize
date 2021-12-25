using System;

namespace Schemata;

public partial class Connector
{
    public class ProcessingEventArgs : EventArgs
    {
        public ProcessingEventArgs(Model model)
        {
            Model = model;
        }

        public Model Model { get; }
    }
}
