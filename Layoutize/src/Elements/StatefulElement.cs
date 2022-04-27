using System;
using System.Diagnostics;

namespace Layoutize.Elements;

internal sealed class StatefulElement : ComponentElement
{
    internal StatefulElement(StatefulLayout layout)
        : base(layout)
    {
        State = Layout.CreateState();
        State.StateUpdating += UpdateChild;
    }

    private protected override Layout Build()
    {
        Debug.Assert(!IsDisposed);
        return State.Build(this);
    }

    private protected override void Dispose(bool disposing)
    {
        if (!IsDisposed && disposing)
        {
            State.Dispose();
        }
        base.Dispose(disposing);
    }

    private protected override void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        State.Layout = Layout;
        base.OnLayoutUpdated(e);
    }

    private new StatefulLayout Layout => Layout;

    private State State { get; }

    private void UpdateChild(object? sender, EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        Layout newChildLayout = Build();
        if (Child.Layout.GetType().Equals(newChildLayout.GetType()))
        {
            Child.Layout = newChildLayout;
        }
        else
        {
            Child = newChildLayout.CreateElement();
        }
    }
}
