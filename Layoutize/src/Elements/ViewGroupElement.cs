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
			Debug.Assert(IsMounted);
		}
	}

	public override bool IsMounted
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

	protected ViewGroupElement(ViewGroupLayout layout)
		: base(layout)
	{
		ViewModel.LayoutUpdated += (sender, e) => UpdateChildren();
	}

	protected override void OnMounting(EventArgs e)
	{
		base.OnMounting(e);
		var children = Layout.Children.Select(childLayout => childLayout.CreateElement()).ToImmutableSortedSet();
		foreach (var child in children)
		{
			child.Mount(this);
		}
	}

	protected override void OnUnmounted(EventArgs e)
	{
		foreach (var child in Children)
		{
			child.Unmount();
		}
		_children = ImmutableSortedSet<Element>.Empty;
		base.OnUnmounted(e);
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
