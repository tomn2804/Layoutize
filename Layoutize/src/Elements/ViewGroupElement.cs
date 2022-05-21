using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Layoutize.Elements;

internal abstract partial class ViewGroupElement : ViewElement
{
    private protected ViewGroupElement(ViewGroupLayout layout)
        : base(layout)
    {
        _children = new(() => Layout.Children.Select(layout => layout.CreateElement()).ToImmutableSortedSet());
    }

    internal new bool IsMounted
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

    private protected override void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(IsMounted);
        ImmutableSortedSet<Element>.Builder childrenBuilder = ImmutableSortedSet.CreateBuilder<Element>();
        foreach (Element newChild in Layout.Children.Select(layout => layout.CreateElement()).ToImmutableSortedSet())
        {
            if (Children.TryGetValue(newChild, out Element? currentChild) && Comparer.Equals(currentChild, newChild))
            {
                currentChild.Layout = newChild.Layout;
                childrenBuilder.Add(currentChild);
            }
            else
            {
                childrenBuilder.Add(newChild);
            }
        }
        if (childrenBuilder.Any())
        {
            Children = childrenBuilder.ToImmutable();
        }
        Debug.Assert(IsMounted);
        base.OnLayoutUpdated(e);
    }

    private protected override void OnMounting(EventArgs e)
    {
        base.OnMounting(e);
        Debug.Assert(!IsMounted);
        foreach (Element child in Children)
        {
            child.Mount(this);
        }
        Debug.Assert(IsMounted);
    }

    private protected override void OnUnmounting(EventArgs e)
    {
        base.OnUnmounting(e);
        Debug.Assert(IsMounted);
        foreach (Element child in Children)
        {
            child.Unmount();
        }
        Debug.Assert(!IsMounted);
    }
}

internal abstract partial class ViewGroupElement
{
    private Lazy<ImmutableSortedSet<Element>> _children;

    internal event EventHandler? ChildrenUpdated;

    internal event EventHandler? ChildrenUpdating;

    internal ImmutableSortedSet<Element> Children
    {
        get => _children.Value;
        private protected set
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

    private protected virtual void OnChildrenUpdated(EventArgs e)
    {
        Debug.Assert(IsMounted);
        ChildrenUpdated?.Invoke(this, e);
    }

    private protected virtual void OnChildrenUpdating(EventArgs e)
    {
        Debug.Assert(IsMounted);
        ChildrenUpdating?.Invoke(this, e);
    }
}
