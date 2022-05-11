using Layoutize.Attributes;
using Layoutize.Views;
using System;
using System.Diagnostics;

namespace Layoutize.Elements;

internal abstract partial class ViewElement : Element
{
    private readonly Lazy<View> _view;

    private protected ViewElement(ViewLayout layout)
        : base(layout)
    {
        _view = new(() => ViewLayout.CreateView(this));
        Creating += Attributes.OnCreating.Of(this);
        Created += Attributes.OnCreated.Of(this);
        Deleting += Attributes.OnDeleting.Of(this);
        Deleted += Attributes.OnDeleted.Of(this);
        Mounting += Attributes.OnMounting.Of(this);
        Mounted += Attributes.OnMounted.Of(this);
        Unmounting += Attributes.OnUnmounting.Of(this);
        Unmounted += Attributes.OnUnmounted.Of(this);
    }

    internal override View View => _view.Value;

    private Action? Cleanup { get; set; }

    private ViewLayout ViewLayout => (ViewLayout)Layout;

    private protected virtual Action? Build()
    {
        return null;
    }

    private protected override void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        Rebuild();
        base.OnLayoutUpdated(e);
    }

    private protected override void OnMounting(EventArgs e)
    {
        base.OnMounting(e);
        Debug.Assert(!IsDisposed);
        Cleanup = Build();
        Create();
        Debug.Assert(View.Exists);
    }

    private protected override void OnUnmounting(EventArgs e)
    {
        base.OnUnmounting(e);
        Debug.Assert(!IsDisposed);
        bool deleteOnUnmount = DeleteOnUnmount.Of(this) ?? false;
        if (deleteOnUnmount && View.Exists)
        {
            Delete();
        }
    }

    private void Rebuild()
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        Cleanup?.Invoke();
        Cleanup = Build();
    }
}

internal abstract partial class ViewElement
{
    internal event EventHandler? Created;

    internal event EventHandler? Creating;

    private protected virtual void OnCreated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(View.Exists);
        Created?.Invoke(this, e);
    }

    private protected virtual void OnCreating(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(!View.Exists);
        Creating?.Invoke(this, e);
    }

    private void Create()
    {
        Debug.Assert(!IsDisposed);
        if (!View.Exists)
        {
            OnCreating(EventArgs.Empty);
            View.Create();
            OnCreated(EventArgs.Empty);
        }
    }
}

internal abstract partial class ViewElement
{
    internal event EventHandler? Deleted;

    internal event EventHandler? Deleting;

    private protected virtual void OnDeleted(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(!View.Exists);
        Deleted?.Invoke(this, e);
    }

    private protected virtual void OnDeleting(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(View.Exists);
        Deleting?.Invoke(this, e);
    }

    private void Delete()
    {
        Debug.Assert(!IsDisposed);
        if (View.Exists)
        {
            OnDeleting(EventArgs.Empty);
            View.Delete();
            OnDeleted(EventArgs.Empty);
        }
    }
}
