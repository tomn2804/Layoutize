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
		get => _children.Value;
		protected set
		{
			Debug.Assert(IsMounted);
			Debug.Assert(value.All(child => child.Parent == this));
			IEnumerable<Element> enteringChildren = value.Except(Children);
			IEnumerable<Element> exitingChildren = Children.Except(value);
			foreach (var exitingChild in exitingChildren)
			{
				exitingChild.Unmount();
			}
			Debug.Assert(exitingChildren.All(child => !child.IsMounted));
			Debug.Assert(exitingChildren.All(child => child.Parent == null));
			_children = new(() => value);
			foreach (var enteringChild in enteringChildren)
			{
				enteringChild.Mount();
			}
			Debug.Assert(IsMounted);
		}
	}

	public new bool IsMounted
	{
		get
		{
			if (base.IsMounted)
			{
				Debug.Assert(Children.Count == Layout.Children.Count());
				Debug.Assert(Children.All(child => child.IsMounted));
				Debug.Assert(Children.All(child => child.Parent == this));
				return true;
			}
			return false;
		}
	}

	protected ViewGroupElement(Element? parent, ViewGroupLayout layout)
		: base(parent, layout)
	{
		_children = new(
			() => Layout.Children.Select(childLayout => childLayout.CreateElement(this)).ToImmutableSortedSet()
		);
	}

	protected override void OnLayoutUpdated(EventArgs e)
	{
		Debug.Assert(IsMounted);
		Rebuild();
		base.OnLayoutUpdated(e);
	}

	protected override void OnMounting(EventArgs e)
	{
		base.OnMounting(e);
		foreach (var child in Children)
		{
			child.Mount();
		}
		Debug.Assert(!IsMounted);
	}

	protected override void OnUnmounted(EventArgs e)
	{
		Debug.Assert(!IsMounted);
		foreach (var child in Children)
		{
			child.Unmount();
		}
		_children = new(
			() => Layout.Children.Select(childLayout => childLayout.CreateElement(this)).ToImmutableSortedSet()
		);
		base.OnUnmounted(e);
	}

	private void Rebuild()
	{
		Debug.Assert(IsMounted);
		var childrenBuilder = ImmutableSortedSet.CreateBuilder<Element>();
		foreach (var newChildLayout in Layout.Children)
		{
			var newChild = newChildLayout.CreateElement(this);
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

	private Lazy<ImmutableSortedSet<Element>> _children;
}
