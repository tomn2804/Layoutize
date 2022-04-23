using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Layoutize;

public abstract class ViewElement : Element
{
    protected View? View { get; private set; }

    protected new ViewLayout Layout => Layout;

    protected ViewElement(ViewLayout layout)
        : base(layout)
    {
    }

    protected internal override void Mount(Element? parent)
    {
        Debug.Assert(!IsDisposed);
        base.Mount(parent);
        View = Layout.CreateView(this);
        //View.Mount();
    }

    protected internal override void Unmount()
    {
        Debug.Assert(!IsDisposed);
        //View.Unmount();
        base.Unmount();
    }

    protected override void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        //Debug.Assert(View.IsMounted);
        //View.Name = (string)Layout.Attributes["Name"];
        base.OnLayoutUpdated(e);
    }
}

public abstract partial class ViewGroupElement : ViewElement
{
    protected ViewGroupElement(ViewGroupLayout layout)
        : base(layout)
    {
    }
}

public abstract partial class ViewGroupElement
{
    private ImmutableHashSet<Element> _children = ImmutableHashSet<Element>.Empty;

    public ImmutableHashSet<Element> Children
    {
        get => _children;
        private protected set
        {
            Debug.Assert(!IsDisposed);
            OnChildrenUpdating(EventArgs.Empty);
            ImmutableHashSet<Element> enteringChildren = value.Except(_children);
            ImmutableHashSet<Element> exitingChildren = _children.Except(value);
            foreach (Element exitingChild in exitingChildren)
            {
                exitingChild.Unmount();
            }
            _children = value;
            foreach (Element enteringChild in enteringChildren)
            {
                enteringChild.Mount(this);
            }
            OnChildrenUpdated(EventArgs.Empty);
        }
    }

    public event EventHandler? ChildrenUpdating;

    public event EventHandler? ChildrenUpdated;

    protected virtual void OnChildrenUpdating(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        ChildrenUpdating?.Invoke(this, e);
    }

    protected virtual void OnChildrenUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        ChildrenUpdated?.Invoke(this, e);
    }
}

public class FileElement : ViewElement
{
    protected new FileLayout Layout => Layout;

    protected new FileView? View => View;

    protected FileElement(FileLayout layout)
        : base(layout)
    {
    }
}

public class DirectoryElement : ViewGroupElement
{
    protected new DirectoryLayout Layout => Layout;

    protected new DirectoryView? View => View;

    protected DirectoryElement(DirectoryLayout layout)
        : base(layout)
    {
    }

    protected internal override void Mount(Element? parent)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(View != null);
        if (Layout.Attributes.TryGetValue("Children", out object? childrenObject) && childrenObject is IEnumerable<Layout> children)
        {
            View.Mounted += (sender, e) =>
            {
                Children = Children.Union(children.Select(layout =>
                {
                    ImmutableDictionary<object, object>.Builder scope = ImmutableDictionary.CreateBuilder();
                    scope.Add("Path", View.FullName);
                    scope.Add("Child", layout);
                    return new ScopeLayout(scope).CreateElement();
                }));
            };
        }
        base.Mount(parent);
    }
}
