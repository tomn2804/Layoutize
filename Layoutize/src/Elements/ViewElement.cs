using Layoutize.Views;
using System;
using System.Diagnostics;

namespace Layoutize.Elements;

internal abstract partial class ViewElement : Element
{
    private readonly Lazy<View> _view;

    protected ViewElement(ViewLayout layout)
        : base(layout)
    {
        _view = new(() => Layout.CreateView(this));
        AddEventHandler();
    }

    public new ViewLayout Layout => (ViewLayout)base.Layout;

    public override View View => _view.Value;

    protected override void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(IsMounted);
        AddEventHandler();
        base.OnLayoutUpdated(e);
    }

    protected override void OnLayoutUpdating(EventArgs e)
    {
        base.OnLayoutUpdating(e);
        Debug.Assert(IsMounted);
        RemoveEventHandler();
    }

    protected override void OnMounting(EventArgs e)
    {
        base.OnMounting(e);
        Debug.Assert(!IsMounted);
        if (!View.Exists)
        {
            Create();
        }
    }

    protected override void OnUnmounting(EventArgs e)
    {
        base.OnUnmounting(e);
        Debug.Assert(IsMounted);
        if (Layout.DeleteOnUnmount && View.Exists)
        {
            Delete();
        }
    }

    private void AddEventHandler()
    {
        Creating += Layout.OnCreating;
        Created += Layout.OnCreated;
        Deleting += Layout.OnDeleting;
        Deleted += Layout.OnDeleted;
        Mounting += Layout.OnMounting;
        Mounted += Layout.OnUnmounted;
        Unmounting += Layout.OnUnmounting;
        Unmounted += Layout.OnUnmounted;
    }

    private void RemoveEventHandler()
    {
        Creating -= Layout.OnCreating;
        Created -= Layout.OnCreated;
        Deleting -= Layout.OnDeleting;
        Deleted -= Layout.OnDeleted;
        Mounting -= Layout.OnMounting;
        Mounted -= Layout.OnUnmounted;
        Unmounting -= Layout.OnUnmounting;
        Unmounted -= Layout.OnUnmounted;
    }
}

internal abstract partial class ViewElement
{
    public event EventHandler? Created;

    public event EventHandler? Creating;

    protected virtual void OnCreated(EventArgs e)
    {
        Debug.Assert(View.Exists);
        Created?.Invoke(this, e);
    }

    protected virtual void OnCreating(EventArgs e)
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
    public event EventHandler? Deleted;

    public event EventHandler? Deleting;

    protected virtual void OnDeleted(EventArgs e)
    {
        Debug.Assert(!View.Exists);
        Deleted?.Invoke(this, e);
    }

    protected virtual void OnDeleting(EventArgs e)
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
