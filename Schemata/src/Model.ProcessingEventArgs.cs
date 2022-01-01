using System;

namespace Schemata;

public abstract partial class Model
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
