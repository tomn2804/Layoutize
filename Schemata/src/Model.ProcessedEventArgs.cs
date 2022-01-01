using System;

namespace Schemata;

public abstract partial class Model
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
