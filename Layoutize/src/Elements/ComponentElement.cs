using Layoutize.Views;
using System;
using System.Diagnostics;

namespace Layoutize.Elements;

internal abstract partial class ComponentElement : Element
{
    private protected ComponentElement(Layout layout)
        : base(layout)
    {
        _child = new(() => Build().CreateElement());
    }

    internal new bool IsMounted
    {
        get
        {
            if (base.IsMounted)
            {
                Debug.Assert(Parent != null);
                Debug.Assert(Child.IsMounted);
                Debug.Assert(Child.Parent == this);
                return true;
            }
            Debug.Assert(Parent == null);
            Debug.Assert(!Child.IsMounted);
            Debug.Assert(Child.Parent == null);
            return false;
        }
    }

    internal override View View => Child.View;

    private protected abstract Layout Build();

    private protected override void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(IsMounted);
        Child.Layout = Build();
        Debug.Assert(IsMounted);
        base.OnLayoutUpdated(e);
    }

    private protected override void OnMounting(EventArgs e)
    {
        base.OnMounting(e);
        Debug.Assert(!IsMounted);
        Child.Mount(this);
        Debug.Assert(IsMounted);
    }

    private protected override void OnUnmounting(EventArgs e)
    {
        base.OnUnmounting(e);
        Debug.Assert(IsMounted);
        Child.Unmount();
        Debug.Assert(!IsMounted);
    }
}

internal abstract partial class ComponentElement
{
    private Lazy<Element> _child;

    internal event EventHandler? ChildUpdated;

    internal event EventHandler? ChildUpdating;

    internal Element Child
    {
        get => _child.Value;
        private protected set
        {
            Debug.Assert(IsMounted);
            OnChildUpdating(EventArgs.Empty);
            Child.Unmount();
            _child = new(() => value);
            Child.Mount(this);
            OnChildUpdated(EventArgs.Empty);
            Debug.Assert(IsMounted);
        }
    }

    private protected virtual void OnChildUpdated(EventArgs e)
    {
        Debug.Assert(IsMounted);
        ChildUpdated?.Invoke(this, e);
    }

    private protected virtual void OnChildUpdating(EventArgs e)
    {
        Debug.Assert(IsMounted);
        ChildUpdating?.Invoke(this, e);
    }
}
