using Layoutize.Views;
using System;
using System.Diagnostics;

namespace Layoutize.Elements;

internal abstract partial class ComponentElement : Element
{
    internal override bool IsMounted
    {
        get
        {
            if (base.IsMounted)
            {
                Debug.Assert(Child.IsMounted);
                Debug.Assert(Child.Parent == this);
                return true;
            }
            return false;
        }
    }

    internal override View View => Child.View;

    private protected ComponentElement(Layout layout)
        : base(layout)
    {
        _child = new(() => Build().CreateElement());
    }

    private protected abstract Layout Build();

    private protected override void Dispose(bool disposing)
    {
        if (!IsDisposed && disposing)
        {
            Child.Dispose();
        }
        base.Dispose(disposing);
    }

    private protected override void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        Child.Layout = Build();
        base.OnLayoutUpdated(e);
    }

    private protected override void OnMounting(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        base.OnMounting(e);
        Child.Mount(this);
        Debug.Assert(Child.IsMounted);
        Debug.Assert(Child.Parent == this);
    }

    private protected override void OnUnmounting(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        base.OnUnmounting(e);
        if (Child.IsMounted) Child.Unmount();
        Debug.Assert(!Child.IsMounted);
        Debug.Assert(Child.Parent == null);
    }
}

internal abstract partial class ComponentElement
{
    internal event EventHandler? ChildUpdated;

    internal event EventHandler? ChildUpdating;

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
            Child.Mount(this);
            OnChildUpdated(EventArgs.Empty);
            Debug.Assert(Child.IsMounted);
            Debug.Assert(Child.Parent == this);
        }
    }

    private protected virtual void OnChildUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        ChildUpdated?.Invoke(this, e);
    }

    private protected virtual void OnChildUpdating(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        ChildUpdating?.Invoke(this, e);
    }

    private Lazy<Element> _child;
}
