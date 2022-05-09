using Layoutize.Views;
using System;
using System.Diagnostics;

namespace Layoutize.Elements;

internal abstract class ViewElement : Element
{
    private readonly Lazy<View> _view;

    private protected ViewElement(ViewLayout layout)
        : base(layout)
    {
        _view = new(() => ViewLayout.CreateView(this));
    }

    internal override View View => _view.Value;

    private ViewLayout ViewLayout => (ViewLayout)Layout;

    private protected override void OnMounting(EventArgs e)
    {
        base.OnMounting(e);
        Debug.Assert(!IsDisposed);
        if (!View.Exists)
        {
            View.Create();
        }
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
                View.Delete();
            }
        }
    }
}
