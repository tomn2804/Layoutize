﻿using System;
using System.Diagnostics;

namespace Layoutize.Elements;

internal sealed class StatefulElement : ComponentElement
{
    private readonly State _state;

    public StatefulElement(StatefulLayout layout)
        : base(layout)
    {
        _state = Layout.CreateState();
        _state.Element = this;
        _state.StateUpdated += UpdateChild;
    }

    private new StatefulLayout Layout => (StatefulLayout)base.Layout;

    protected override Layout Build()
    {
        return _state.Build(this);
    }

    private void UpdateChild(object? sender, EventArgs e)
    {
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
        Debug.Assert(IsMounted);
    }
}
