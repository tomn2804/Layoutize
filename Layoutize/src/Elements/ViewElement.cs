using Layoutize.Attributes;
using Layoutize.Views;
using System;
using System.Diagnostics;

namespace Layoutize.Elements;

internal abstract partial class ViewElement : Element
{
    private Lazy<View> _view;

    private protected ViewElement(ViewLayout layout)
        : base(layout)
    {
        _view = new(() => Layout.CreateView(this));
        Creating += Layout.OnCreating;
        Created += Layout.OnCreated;
        Deleting += Layout.OnDeleting;
        Deleted += Layout.OnDeleted;
        Mounting += Layout.OnMounting;
        Mounted += Layout.OnUnmounted;
        Unmounting += Layout.OnUnmounting;
        Unmounted += Layout.OnUnmounted;
    }

    internal override View View => _view.Value;

    private new ViewLayout Layout => (ViewLayout)base.Layout;

    private protected override void OnLayoutUpdated(EventArgs e)
    {
        Creating += Layout.OnCreating;
        Created += Layout.OnCreated;
        Deleting += Layout.OnDeleting;
        Deleted += Layout.OnDeleted;
        Mounting += Layout.OnMounting;
        Mounted += Layout.OnUnmounted;
        Unmounting += Layout.OnUnmounting;
        Unmounted += Layout.OnUnmounted;
        base.OnLayoutUpdated(e);
    }

    private protected override void OnLayoutUpdating(EventArgs e)
    {
        base.OnLayoutUpdating(e);
        Creating -= Layout.OnCreating;
        Created -= Layout.OnCreated;
        Deleting -= Layout.OnDeleting;
        Deleted -= Layout.OnDeleted;
        Mounting -= Layout.OnMounting;
        Mounted -= Layout.OnUnmounted;
        Unmounting -= Layout.OnUnmounting;
        Unmounted -= Layout.OnUnmounted;
    }

    private protected override void OnMounting(EventArgs e)
    {
        base.OnMounting(e);
        Debug.Assert(!IsMounted);
        if (!View.Exists)
        {
            Create();
        }
        Debug.Assert(IsMounted);
    }

    private protected override void OnUnmounting(EventArgs e)
    {
        base.OnUnmounting(e);
        Debug.Assert(IsMounted);
        if (Layout.DeleteOnUnmount && View.Exists)
        {
            Delete();
        }
        Debug.Assert(!IsMounted);
    }
}

internal abstract partial class ViewElement
{
    internal event EventHandler? Created;

    internal event EventHandler? Creating;

    private protected virtual void OnCreated(EventArgs e)
    {
        Debug.Assert(View.Exists);
        Created?.Invoke(this, e);
    }

    private protected virtual void OnCreating(EventArgs e)
    {
        Debug.Assert(!View.Exists);
        Creating?.Invoke(this, e);
    }

    private void Create()
    {
        Debug.Assert(!View.Exists);
        OnCreating(EventArgs.Empty);
        View.Create();
        OnCreated(EventArgs.Empty);
        Debug.Assert(View.Exists);
    }
}

internal abstract partial class ViewElement
{
    internal event EventHandler? Deleted;

    internal event EventHandler? Deleting;

    private protected virtual void OnDeleted(EventArgs e)
    {
        Debug.Assert(!View.Exists);
        Deleted?.Invoke(this, e);
    }

    private protected virtual void OnDeleting(EventArgs e)
    {
        Debug.Assert(View.Exists);
        Deleting?.Invoke(this, e);
    }

    private void Delete()
    {
        Debug.Assert(View.Exists);
        OnDeleting(EventArgs.Empty);
        View.Delete();
        OnDeleted(EventArgs.Empty);
        Debug.Assert(!View.Exists);
    }
}
