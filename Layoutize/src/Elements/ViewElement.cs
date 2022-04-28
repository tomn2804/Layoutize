using Layoutize.Views;
using System;
using System.Diagnostics;

namespace Layoutize.Elements;

internal abstract class ViewElement : Element
{
    internal View View => _view.Value;

    internal override void MountTo(Element? parent)
    {
        Debug.Assert(!IsDisposed);
        base.MountTo(parent);
        if (!View.Exists) View.Create();
    }

    internal override void Unmount()
    {
        Debug.Assert(!IsDisposed);
        if (Layout.Attributes.TryGetValue("DeleteOnUnmount", out object? deleteOnUnmountObject))
        {
            bool deleteOnUnmount = (bool)deleteOnUnmountObject;
            if (deleteOnUnmount && View.Exists) View.Delete();
        }
        base.Unmount();
    }

    private protected ViewElement(ViewLayout layout)
        : base(layout)
    {
        _view = new(() => ViewLayout.CreateView(this));
    }

    private protected override void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        View.Name = (string)Layout.Attributes["Name"];
        base.OnLayoutUpdated(e);
    }

    private readonly Lazy<View> _view;

    private ViewLayout ViewLayout => (ViewLayout)Layout;
}
