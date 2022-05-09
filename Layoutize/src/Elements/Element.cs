using Layoutize.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Layoutize.Elements;

internal abstract partial class Element
{
    internal string Name => (string)Layout.Attributes["Name"];

    internal virtual bool IsMounted { get; private set; }

    internal Element? Parent { get; private set; }

    internal abstract View View { get; }

    internal virtual void MountTo(Element? parent)
    {
        Debug.Assert(!IsDisposed);
        if (IsMounted) Unmount();
        Parent = parent;
        IsMounted = true;
    }

    internal virtual void Unmount()
    {
        Debug.Assert(!IsDisposed);
        Parent = null;
        IsMounted = false;
    }

    private protected Element(Layout layout)
    {
        Debug.Assert(layout.Attributes.ContainsKey("Name"));
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

internal abstract partial class Element : IComparable<Element>
{
    public int CompareTo(Element? other)
    {
        return Name.CompareTo(other?.Name);
    }
}

internal abstract partial class Element
{
    private protected sealed partial class UpdateComparer : IEqualityComparer<Element>
    {
        public bool Equals(Element? x, Element? y)
        {
            if (x == null || y == null)
            {
                return x == y;
            }
            return x.Layout.GetType().Equals(y.Layout.GetType()) && x.CompareTo(y).Equals(0);
        }

        public int GetHashCode(Element obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
