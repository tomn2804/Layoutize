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
        _view = new(() => ViewLayout.CreateView(this));
        Creating += Attributes.OnCreating.Of(Layout.Attributes);
        Created += Attributes.OnCreated.Of(Layout.Attributes);
        Deleting += Attributes.OnDeleting.Of(Layout.Attributes);
        Deleted += Attributes.OnDeleted.Of(Layout.Attributes);
        Mounting += Attributes.OnMounting.Of(Layout.Attributes);
        Mounted += Attributes.OnMounted.Of(Layout.Attributes);
        Unmounting += Attributes.OnUnmounting.Of(Layout.Attributes);
        Unmounted += Attributes.OnUnmounted.Of(Layout.Attributes);
    }

    internal override View View => _view.Value;

    private ViewLayout ViewLayout => (ViewLayout)Layout;

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
        bool deleteOnUnmount = DeleteOnUnmount.Of(Layout.Attributes) ?? false;
        if (deleteOnUnmount && View.Exists)
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
