using Layoutize.Views;
using System;
using System.Diagnostics;

namespace Layoutize.Elements;

internal abstract class ViewElement : Element
{
    internal override View View => _view.Value;

    private protected ViewElement(ViewLayout layout)
        : base(layout)
    {
        _view = new(() => ViewLayout.CreateView(this));
    }

    private protected override void OnMounting(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        base.OnMounting(e);
        if (!View.Exists) View.Create();
        Debug.Assert(View.Exists);
    }

    private protected override void OnUnmounting(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        base.OnUnmounting(e);
        if (Layout.Attributes.TryGetValue("DeleteOnUnmount", out object? deleteOnUnmountObject))
        {
            bool deleteOnUnmount = (bool)deleteOnUnmountObject;
            if (deleteOnUnmount && View.Exists) View.Delete();
        }
    }

    private readonly Lazy<View> _view;

    private ViewLayout ViewLayout => (ViewLayout)Layout;
}
