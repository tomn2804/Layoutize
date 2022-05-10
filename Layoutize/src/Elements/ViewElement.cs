using Layoutize.Views;
using System;
using System.Diagnostics;
using System.Management.Automation;

namespace Layoutize.Elements;

internal abstract partial class ViewElement : Element
{
    private readonly Lazy<View> _view;

    private protected ViewElement(ViewLayout layout)
        : base(layout)
    {
        _view = new(() => ViewLayout.CreateView(this));
        HandleEventSubscription();
    }

    internal override View View => _view.Value;

    private ViewLayout ViewLayout => (ViewLayout)Layout;

    private protected override void OnLayoutUpdated(EventArgs e)
    {
        base.OnLayoutUpdated(e);
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        HandleEventSubscription();
    }

    private protected override void OnLayoutUpdating(EventArgs e)
    {
        base.OnLayoutUpdating(e);
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        HandleEventUnsubsciption();
    }

    private protected override void OnMounting(EventArgs e)
    {
        base.OnMounting(e);
        Debug.Assert(!IsDisposed);
        Create();
        Debug.Assert(View.Exists);
    }

    private protected override void OnUnmounting(EventArgs e)
    {
        base.OnUnmounting(e);
        Debug.Assert(!IsDisposed);
        if (Layout.Attributes.TryGetValue("DeleteOnUnmount", out object? deleteOnUnmountObject))
        {
            bool deleteOnUnmount = (bool)deleteOnUnmountObject;
            if (deleteOnUnmount && View.Exists)
            {
                Delete();
            }
        }
    }

    private EventHandler? GetEventHandlerAttribute(string key)
    {
        if (Layout.Attributes.TryGetValue(key, out object? scriptBlockObject))
        {
            ScriptBlock scriptBlock = (ScriptBlock)scriptBlockObject;
            return (sender, e) => scriptBlock.Invoke(sender, e);
        }
        return null;
    }

    private void HandleEventSubscription()
    {
        Creating += GetEventHandlerAttribute("OnCreating");
        Created += GetEventHandlerAttribute("OnCreated");
        Deleting += GetEventHandlerAttribute("OnDeleting");
        Deleted += GetEventHandlerAttribute("OnDeleted");
        Mounting += GetEventHandlerAttribute("OnMounting");
        Mounted += GetEventHandlerAttribute("OnMounted");
        Unmounting += GetEventHandlerAttribute("OnUnmounting");
        Unmounted += GetEventHandlerAttribute("OnUnmounted");
    }

    private void HandleEventUnsubsciption()
    {
        Creating -= GetEventHandlerAttribute("OnCreating");
        Created -= GetEventHandlerAttribute("OnCreated");
        Deleting -= GetEventHandlerAttribute("OnDeleting");
        Deleted -= GetEventHandlerAttribute("OnDeleted");
        Mounting -= GetEventHandlerAttribute("OnMounting");
        Mounted -= GetEventHandlerAttribute("OnMounted");
        Unmounting -= GetEventHandlerAttribute("OnUnmounting");
        Unmounted -= GetEventHandlerAttribute("OnUnmounted");
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
