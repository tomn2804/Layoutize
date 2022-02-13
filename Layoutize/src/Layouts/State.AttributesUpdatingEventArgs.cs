using System;

namespace Layoutize.Layouts;

public abstract partial class State
{
    public class UpdatedEventArgs : EventArgs
    {
        public Layout NewLayout { get; }

        public UpdatedEventArgs(Layout newLayout)
        {
            NewLayout = newLayout;
        }
    }
}
