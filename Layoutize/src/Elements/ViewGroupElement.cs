using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Layoutize.Elements;

internal abstract partial class ViewGroupElement : ViewElement
{
    protected ViewGroupElement(ViewGroupLayout layout)
        : base(layout)
    {
        _children = new(() => Layout.Children.Select(layout => layout.CreateElement()).ToImmutableSortedSet());
    }

    public new bool IsMounted
    {
        get
        {
            if (base.IsMounted)
            {
                Debug.Assert(Children.All(child => child.IsMounted));
                Debug.Assert(Children.All(child => child.Parent == this));
                return true;
            }
            Debug.Assert(Children.All(child => !child.IsMounted));
            Debug.Assert(Children.All(child => child.Parent == null));
            return false;
        }
    }

    private new ViewGroupLayout Layout => (ViewGroupLayout)base.Layout;

    protected override void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(IsMounted);
        ImmutableSortedSet<Element>.Builder childrenBuilder = ImmutableSortedSet.CreateBuilder<Element>();
        foreach (Layout newChildLayout in Layout.Children)
        {
            Element newChild = newChildLayout.CreateElement();
            if (Children.TryGetValue(newChild, out Element? currentChild) && currentChild.Layout.GetType().Equals(newChildLayout.GetType()))
            {
                currentChild.Layout = newChildLayout;
                childrenBuilder.Add(currentChild);
            }
            else
            {
                childrenBuilder.Add(newChild);
            }
        }
        Children = childrenBuilder.ToImmutable();
        Debug.Assert(IsMounted);
        base.OnLayoutUpdated(e);
    }

    protected override void OnMounting(EventArgs e)
    {
        base.OnMounting(e);
        Debug.Assert(!IsMounted);
        foreach (Element child in Children)
        {
            child.Mount(this);
        }
    }

    protected override void OnUnmounting(EventArgs e)
    {
        base.OnUnmounting(e);
        Debug.Assert(IsMounted);
        foreach (Element child in Children)
        {
            child.Unmount();
        }
    }
}

internal abstract partial class ViewGroupElement
{
    private Lazy<ImmutableSortedSet<Element>> _children;

    public event EventHandler? ChildrenUpdated;

    public event EventHandler? ChildrenUpdating;

    public ImmutableSortedSet<Element> Children
    {
        get => _children.Value;
        protected set
        {
            Debug.Assert(IsMounted);
            OnChildrenUpdating(EventArgs.Empty);
            IEnumerable<Element> enteringChildren = value.Except(Children);
            IEnumerable<Element> exitingChildren = Children.Except(value);
            foreach (Element exitingChild in exitingChildren)
            {
                exitingChild.Unmount();
            }
            Debug.Assert(exitingChildren.All(child => !child.IsMounted));
            Debug.Assert(exitingChildren.All(child => child.Parent == null));
            _children = new(() => value);
            foreach (Element enteringChild in enteringChildren)
            {
                enteringChild.Mount(this);
            }
            OnChildrenUpdated(EventArgs.Empty);
            Debug.Assert(IsMounted);
        }
    }

    protected virtual void OnChildrenUpdated(EventArgs e)
    {
        Debug.Assert(IsMounted);
        ChildrenUpdated?.Invoke(this, e);
    }

    protected virtual void OnChildrenUpdating(EventArgs e)
    {
        Debug.Assert(IsMounted);
        ChildrenUpdating?.Invoke(this, e);
    }
}
