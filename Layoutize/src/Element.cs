using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Layoutize;

public abstract partial class Element : IBuildContext
{
    public Element? Parent { get; private set; }

    public virtual ImmutableDictionary<object, object> Scope => Parent?.Scope ?? ImmutableDictionary<object, object>.Empty;

    protected Element(Layout layout)
    {
        _layout = layout;
    }

    protected internal virtual void Mount(Element? parent)
    {
        Debug.Assert(!IsDisposed);
        Parent = parent;
    }

    protected internal virtual void Unmount()
    {
        Debug.Assert(!IsDisposed);
        Parent = null;
    }
}

public abstract partial class Element
{
    public event EventHandler? LayoutUpdated;

    public event EventHandler? LayoutUpdating;

    private Layout _layout;

    public Layout Layout
    {
        get => _layout;
        internal set
        {
            Debug.Assert(!IsDisposed);
            OnLayoutUpdating(EventArgs.Empty);
            _layout = value;
            OnLayoutUpdated(EventArgs.Empty);
        }
    }

    protected virtual void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        LayoutUpdated?.Invoke(this, e);
    }

    protected virtual void OnLayoutUpdating(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        LayoutUpdating?.Invoke(this, e);
    }
}

public abstract partial class Element : IDisposable
{
    public bool IsDisposed { get; private set; }

    protected virtual void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                Unmount();
                LayoutUpdated = null;
                LayoutUpdating = null;
            }
            IsDisposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
