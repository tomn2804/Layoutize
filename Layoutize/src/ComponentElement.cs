using System;
using System.Diagnostics;

namespace Layoutize;

internal abstract partial class ComponentElement : Element
{
    internal override bool IsMounted => base.IsMounted && Child.IsMounted;

    private protected ComponentElement(Layout layout)
        : base(layout)
    {
        _child = new(() => Build().CreateElement());
    }

    private protected abstract Layout Build();

    internal override void MountTo(Element? parent)
    {
        Debug.Assert(!IsDisposed);
        base.MountTo(parent);
        Child.MountTo(this);
        Debug.Assert(Child.IsMounted);
        Debug.Assert(Child.Parent == this);
    }

    internal override void Unmount()
    {
        Debug.Assert(!IsDisposed);
        if (Child.IsMounted) Child.Unmount();
        Debug.Assert(!Child.IsMounted);
        Debug.Assert(Child.Parent == null);
        base.Unmount();
    }

    private protected override void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        Child.Layout = Build();
        base.OnLayoutUpdated(e);
    }

    internal override void VisitChildren(Visitor visitor)
    {
        visitor(Child);
    }

    private protected override void Dispose(bool disposing)
    {
        if (!IsDisposed && disposing)
        {
            Child.Dispose();
        }
        base.Dispose(disposing);
    }
}

internal abstract partial class ComponentElement
{
    internal event EventHandler? ChildUpdating;

    internal event EventHandler? ChildUpdated;

    private Lazy<Element> _child;

    internal Element Child
    {
        get => _child.Value;
        private protected set
        {
            Debug.Assert(!IsDisposed);
            Debug.Assert(IsMounted);
            OnChildUpdating(EventArgs.Empty);
            Child.Unmount();
            _child = new(() => value);
            Child.MountTo(this);
            OnChildUpdated(EventArgs.Empty);
            Debug.Assert(Child.IsMounted);
            Debug.Assert(Child.Parent == this);
        }
    }

    private protected virtual void OnChildUpdating(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        ChildUpdating?.Invoke(this, e);
    }

    private protected virtual void OnChildUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        ChildUpdated?.Invoke(this, e);
    }
}

internal sealed partial class StatelessElement : ComponentElement
{
    private new StatelessLayout Layout => Layout;

    internal StatelessElement(StatelessLayout layout)
        : base(layout)
    {
    }

    private protected override sealed Layout Build()
    {
        Debug.Assert(!IsDisposed);
        return Layout.Build(this);
    }
}

internal sealed partial class StatefulElement : ComponentElement
{
    private new StatefulLayout Layout => Layout;

    private State State { get; }

    internal StatefulElement(StatefulLayout layout)
        : base(layout)
    {
        State = Layout.CreateState();
        State.StateUpdating += UpdateChild;
    }

    private protected override void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        State.Layout = Layout;
        base.OnLayoutUpdated(e);
    }

    private void UpdateChild(object? sender, EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        Layout newChildLayout = Build();
        if (Child.Layout.GetType() == newChildLayout.GetType())
        {
            Child.Layout = newChildLayout;
        }
        else
        {
            Child = newChildLayout.CreateElement();
        }
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
}
