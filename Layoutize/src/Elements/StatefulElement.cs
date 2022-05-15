using System;
using System.Diagnostics;

namespace Layoutize.Elements;

internal sealed class StatefulElement : ComponentElement
{
    internal StatefulElement(StatefulLayout layout)
        : base(layout)
    {
        State = StatefulLayout.CreateState();
        State.StateUpdated += UpdateChild;
    }

    private State State { get; }

    private StatefulLayout StatefulLayout => (StatefulLayout)Layout;

    private protected override Layout Build()
    {
        Debug.Assert(!IsDisposed);
        return State.Build(this);
    }

    private protected override void Dispose(bool disposing)
    {
        if (!IsDisposed && disposing)
        {
            Debug.Assert(!State.IsDisposed);
            State.Dispose();
        }
        Debug.Assert(State.IsDisposed);
        base.Dispose(disposing);
    }

    private protected override void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        State.Layout = StatefulLayout;
        base.OnLayoutUpdated(e);
    }

    private void UpdateChild(object? sender, EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        Element newChild = Build().CreateElement();
        if (Comparer.Equals(Child, newChild))
        {
            Child.Layout = newChild.Layout;
        }
        else
        {
            Child = newChild;
        }
    }
}
