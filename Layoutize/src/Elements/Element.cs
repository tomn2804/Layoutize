using Layoutize.Contexts;
using Layoutize.Views;
using System;
using System.Diagnostics;

namespace Layoutize.Elements;

internal abstract partial class Element
{
    private bool _isMounted;

    private protected Element(Layout layout)
    {
        _layout = layout;
    }

    internal bool IsMounted
    {
        get
        {
            if (_isMounted)
            {
                Debug.Assert(View.Exists);
                return true;
            }
            return false;
        }
    }

    internal Element? Parent { get; private set; }

    internal abstract View View { get; }
}

internal abstract partial class Element
{
    private Layout _layout;

    internal event EventHandler? LayoutUpdated;

    internal event EventHandler? LayoutUpdating;

    internal Layout Layout
    {
        get => _layout;
        set
        {
            Debug.Assert(IsMounted);
            OnLayoutUpdating(EventArgs.Empty);
            _layout = value;
            OnLayoutUpdated(EventArgs.Empty);
        }
    }

    private protected virtual void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(IsMounted);
        LayoutUpdated?.Invoke(this, e);
    }

    private protected virtual void OnLayoutUpdating(EventArgs e)
    {
        Debug.Assert(IsMounted);
        LayoutUpdating?.Invoke(this, e);
    }
}

internal abstract partial class Element
{
    internal event EventHandler? Mounted;

    internal event EventHandler? Mounting;

    internal void Mount(Element? parent)
    {
        Debug.Assert(!IsMounted);
        Parent = parent;
        OnMounting(EventArgs.Empty);
        _isMounted = true;
        OnMounted(EventArgs.Empty);
        Debug.Assert(IsMounted);
    }

    private protected virtual void OnMounted(EventArgs e)
    {
        Debug.Assert(IsMounted);
        Mounted?.Invoke(this, e);
    }

    private protected virtual void OnMounting(EventArgs e)
    {
        Debug.Assert(!IsMounted);
        Mounting?.Invoke(this, e);
    }
}

internal abstract partial class Element
{
    internal event EventHandler? Unmounted;

    internal event EventHandler? Unmounting;

    internal void Unmount()
    {
        Debug.Assert(IsMounted);
        OnUnmounting(EventArgs.Empty);
        _isMounted = false;
        OnUnmounted(EventArgs.Empty);
        Parent = null;
        Debug.Assert(!IsMounted);
    }

    private protected virtual void OnUnmounted(EventArgs e)
    {
        Debug.Assert(!IsMounted);
        Unmounted?.Invoke(this, e);
    }

    private protected virtual void OnUnmounting(EventArgs e)
    {
        Debug.Assert(IsMounted);
        Unmounting?.Invoke(this, e);
    }
}

internal abstract partial class Element : IBuildContext
{
    Element IBuildContext.Element => this;
}

internal abstract partial class Element : IComparable<Element>
{
    public int CompareTo(Element? other)
    {
        if (other == null)
        {
            return 1;
        }
        return Name.Of(this).CompareTo(Name.Of(other));
    }
}
