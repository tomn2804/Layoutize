using System;
using System.Diagnostics;

namespace Layoutize.Elements;

internal abstract partial class Element
{
    internal delegate void Visitor(Element element);

    internal virtual bool IsMounted => Parent != null;

    internal Element? Parent { get; private set; }

    internal virtual void MountTo(Element? parent)
    {
        Debug.Assert(!IsDisposed);
        if (IsMounted) Unmount();
        Parent = parent;
    }

    internal virtual void Unmount()
    {
        Debug.Assert(!IsDisposed);
        Parent = null;
    }

    internal virtual void VisitChildren(Visitor visitor)
    {
        Debug.Assert(!IsDisposed);
        return;
    }

    internal virtual void VisitParent(Visitor visitor)
    {
        Debug.Assert(!IsDisposed);
        if (Parent != null) visitor(Parent);
    }

    private protected Element(Layout layout)
    {
        _layout = layout;
    }
}

internal abstract partial class Element
{
    internal event EventHandler? LayoutUpdated;

    internal event EventHandler? LayoutUpdating;

    internal Layout Layout
    {
        get => _layout;
        set
        {
            Debug.Assert(!IsDisposed);
            Debug.Assert(IsMounted);
            OnLayoutUpdating(EventArgs.Empty);
            _layout = value;
            OnLayoutUpdated(EventArgs.Empty);
        }
    }

    private protected virtual void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        LayoutUpdated?.Invoke(this, e);
    }

    private protected virtual void OnLayoutUpdating(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        LayoutUpdating?.Invoke(this, e);
    }

    private Layout _layout;
}

internal abstract partial class Element : IBuildContext
{
    Element IBuildContext.Element => this;
}

internal abstract partial class Element : IDisposable
{
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    internal bool IsDisposed { get; private set; }

    private protected virtual void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                Unmount();
                LayoutUpdating = null;
                LayoutUpdated = null;
            }
            IsDisposed = true;
        }
    }
}
