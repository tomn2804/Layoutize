using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Layoutize.Layouts;

namespace Layoutize.Elements;

internal abstract class ViewGroupElement : ViewElement
{
	[MemberNotNullWhen(true, nameof(Children))]
	public override bool IsMounted
	{
		get
		{
			if (base.IsMounted)
			{
				Debug.Assert(Children != null);
				return true;
			}
			Debug.Assert(Children == null);
			return false;
		}
	}

	protected ViewGroupElement(ViewGroupLayout layout)
		: base(layout)
	{
	}

	[DisallowNull]
	public ImmutableSortedSet<Element>? Children
	{
		get
		{
			if (_children != null)
			{
				Debug.Assert(_children.All(child => child.IsMounted));
				Debug.Assert(_children.All(child => child.Parent == this));
				Debug.Assert(_children.Count == Layout.Children.Count());
			}
			return _children;
		}
		protected set
		{
			Debug.Assert(value.All(child => !child.IsMounted));
			Debug.Assert(value.All(child => child.Parent == null));
			Debug.Assert(IsMounted);
			IEnumerable<Element> enteringChildren = value.Except(Children);
			IEnumerable<Element> exitingChildren = Children.Except(value);
			foreach (var exitingChild in exitingChildren)
			{
				exitingChild.Unmount();
			}
			_children = value;
			foreach (var enteringChild in enteringChildren)
			{
				enteringChild.MountTo(this);
			}
			Debug.Assert(Children == value);
		}
	}

	[MemberNotNull(nameof(Children))]
	protected override Action Mount()
	{
		Debug.Assert(!IsMounted);
		var cleanup = base.Mount();
		_children = Layout.Children.Select(childLayout => childLayout.CreateElement()).ToImmutableSortedSet();
		foreach (var child in _children)
		{
			child.MountTo(this);
		}
		Debug.Assert(Children != null);
		return (() =>
		{
			foreach (var child in Children)
			{
				child.Unmount();
			}
			_children = null;
			Debug.Assert(Children == null);
		}) + cleanup;
	}

	[MemberNotNull(nameof(Children))]
	protected override void OnLayoutUpdated(EventArgs e)
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
		base.OnLayoutUpdated(e);
	}

	private new ViewGroupLayout Layout => (ViewGroupLayout)base.Layout;

	private ImmutableSortedSet<Element>? _children;
}
