using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Layoutize.Layouts;

namespace Layoutize.Elements;

internal abstract class ViewGroupElement : ViewElement
{
	public ImmutableSortedSet<Element> Children
	{
		get => _children;
		protected set
		{
			Debug.Assert(IsMounted);
			OnChildrenUpdating(EventArgs.Empty);
			IEnumerable<Element> enteringChildren = value.Except(Children);
			IEnumerable<Element> exitingChildren = Children.Except(value);
			foreach (var exitingChild in exitingChildren)
			{
				exitingChild.Unmount();
			}
			Debug.Assert(exitingChildren.All(child => !child.IsMounted));
			Debug.Assert(exitingChildren.All(child => child.Parent == null));
			_children = value;
			foreach (var enteringChild in enteringChildren)
			{
				enteringChild.Mount(this);
			}
			OnChildrenUpdated(EventArgs.Empty);
			Debug.Assert(IsMounted);
		}
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

	public event EventHandler? ChildrenUpdated;

	public event EventHandler? ChildrenUpdating;

	protected ViewGroupElement(ViewGroupLayout layout)
		: base(layout)
	{
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

	protected override void OnLayoutUpdated(EventArgs e)
	{
		Debug.Assert(IsMounted);
		UpdateChildren();
		base.OnLayoutUpdated(e);
	}

	protected override void OnMounting(EventArgs e)
	{
		base.OnMounting(e);
		Debug.Assert(!IsMounted);
		foreach (var child in Children)
		{
			child.Mount(this);
		}
	}

	protected override void OnUnmounting(EventArgs e)
	{
		base.OnUnmounting(e);
		Debug.Assert(IsMounted);
		foreach (var child in Children)
		{
			child.Unmount();
		}
	}

	private void UpdateChildren()
	{
		Debug.Assert(IsMounted);
		var childrenBuilder = ImmutableSortedSet.CreateBuilder<Element>();
		foreach (var newChildLayout in Layout.Children)
		{
			var newChild = newChildLayout.CreateElement();
			if (Children.TryGetValue(newChild, out var currentChild)
				&& currentChild.Layout.GetType() == newChildLayout.GetType())
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
	}

	private new ViewGroupLayout Layout => (ViewGroupLayout)base.Layout;

	private ImmutableSortedSet<Element> _children = ImmutableSortedSet<Element>.Empty;
}
