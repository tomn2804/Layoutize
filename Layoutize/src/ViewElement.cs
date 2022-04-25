using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Layoutize;

internal abstract class ViewElement : Element
{
    private readonly Lazy<View> _view;

    internal View View => _view.Value;

    private new ViewLayout Layout => Layout;

    private protected ViewElement(ViewLayout layout)
        : base(layout)
    {
        _view = new(() => Layout.CreateView(this));
    }

    internal override void MountTo(Element? parent)
    {
        Debug.Assert(!IsDisposed);
        base.MountTo(parent);
        if (!View.Exists) View.Create();
    }

    internal override void Unmount()
    {
        Debug.Assert(!IsDisposed);
        if (Layout.Attributes.TryGetValue("DeleteOnUnmount", out object? deleteOnUnmountObject) && deleteOnUnmountObject is bool deleteOnUnmount && deleteOnUnmount)
        {
            if (View.Exists) View.Delete();
        }
        base.Unmount();
    }

    private protected override void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        View.Name = (string)Layout.Attributes["Name"];
        base.OnLayoutUpdated(e);
    }
}

internal abstract partial class ViewGroupElement : ViewElement
{
    internal override bool IsMounted => base.IsMounted && Children.All(child => child.IsMounted);

    private protected ViewGroupElement(ViewGroupLayout layout)
        : base(layout)
    {
        _children = new(() => GetChildrenAttribute());
    }

    private protected override void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        Children = GetChildrenAttribute();
        base.OnLayoutUpdated(e);
    }

    private IImmutableSet<Element> GetChildrenAttribute()
    {
        Debug.Assert(!IsDisposed);
        if (Layout.Attributes.TryGetValue("Children", out object? childrenObject) && childrenObject is IEnumerable<Layout> children)
        {
            return children.Select(layout => layout.CreateElement()).ToImmutableHashSet();
        }
        return ImmutableHashSet<Element>.Empty;
    }

    internal override void VisitChildren(Visitor visitor)
    {
        foreach (Element child in Children)
        {
            visitor(child);
        }
    }

    internal override void MountTo(Element? parent)
    {
        Debug.Assert(!IsDisposed);
        base.MountTo(parent);
        foreach (Element child in Children)
        {
            child.MountTo(this);
        }
        Debug.Assert(Children.All(child => child.IsMounted));
        Debug.Assert(Children.All(child => child.Parent == this));
    }

    internal override void Unmount()
    {
        Debug.Assert(!IsDisposed);
        foreach (Element child in Children)
        {
            child.Unmount();
        }
        Debug.Assert(Children.All(child => !child.IsMounted));
        Debug.Assert(Children.All(child => child.Parent == null));
        base.Unmount();
    }
}

internal abstract partial class ViewGroupElement
{
    private Lazy<IImmutableSet<Element>> _children;

    internal IImmutableSet<Element> Children
    {
        get => _children.Value;
        private protected set
        {
            Debug.Assert(!IsDisposed);
            Debug.Assert(IsMounted);
            OnChildrenUpdating(EventArgs.Empty);
            IImmutableSet<Element> enteringChildren = value.Except(Children);
            IImmutableSet<Element> exitingChildren = Children.Except(value);
            foreach (Element exitingChild in exitingChildren)
            {
                exitingChild.Unmount();
            }
            Debug.Assert(exitingChildren.All(child => !child.IsMounted));
            Debug.Assert(exitingChildren.All(child => child.Parent == null));
            _children = new(() => value);
            foreach (Element enteringChild in enteringChildren)
            {
                enteringChild.MountTo(this);
            }
            Debug.Assert(Children.All(child => child.IsMounted));
            Debug.Assert(Children.All(child => child.Parent == this));
            OnChildrenUpdated(EventArgs.Empty);
        }
    }

    internal event EventHandler? ChildrenUpdating;

    internal event EventHandler? ChildrenUpdated;

    private protected virtual void OnChildrenUpdating(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        ChildrenUpdating?.Invoke(this, e);
    }

    private protected virtual void OnChildrenUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(IsMounted);
        ChildrenUpdated?.Invoke(this, e);
    }
}

internal class FileElement : ViewElement
{
    internal FileElement(FileLayout layout)
        : base(layout)
    {
    }
}

internal class DirectoryElement : ViewGroupElement
{
    internal DirectoryElement(DirectoryLayout layout)
        : base(layout)
    {
    }
}
