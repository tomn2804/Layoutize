using System;
using System.Diagnostics;

namespace Layoutize.Elements;

internal sealed class StatefulElement : ComponentElement
{
    private readonly State _state;

    internal StatefulElement(StatefulLayout layout)
            : base(layout)
    {
        _state = Layout.CreateState();
        _state.StateUpdated += UpdateChild;
    }

    private new StatefulLayout Layout => (StatefulLayout)base.Layout;

    private protected override Layout Build()
    {
        return _state.Build(this);
    }

    private protected override void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(IsMounted);
        _state.Layout = Layout;
        base.OnLayoutUpdated(e);
    }

    private void UpdateChild(object? sender, EventArgs e)
    {
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
        Debug.Assert(IsMounted);
    }
}
