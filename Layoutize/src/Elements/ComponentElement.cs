using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Layoutize.Layouts;
using Layoutize.Views;

namespace Layoutize.Elements;

internal abstract class ComponentElement : Element
{
	[MemberNotNull(nameof(Child), nameof(View))]
	public override void Mount(Element? parent)
	{
		base.Mount(parent);
		Debug.Assert(IsMounted);
	}

	[DisallowNull]
	[NotNullIfNotNull(nameof(_child))]
	public Element? Child
	{
		get => _child;
		protected set
		{
			Debug.Assert(IsMounted);
			Child.Unmount();
			_child = value;
			Child.Mount(this);
			Debug.Assert(IsMounted);
		}
	}

	[MemberNotNullWhen(true, nameof(Child), nameof(View))]
	public override bool IsMounted
	{
		get
		{
			var result = base.IsMounted;
			if (result)
			{
				Debug.Assert(Parent != null);
				Debug.Assert(Child != null);
				Debug.Assert(Child.IsMounted);
				Debug.Assert(Child.Parent == this);
			}
			else
			{
				Debug.Assert(Parent == null);
				Debug.Assert(Child == null);
			}
			return result;
		}
	}

	[NotNullIfNotNull(nameof(Child))]
	public override View? View => Child?.View;

	protected ComponentElement(ComponentLayout layout)
		: base(layout)
	{
	}

	protected abstract Layout Build();

	[MemberNotNull(nameof(Child), nameof(View))]
	protected override void OnLayoutUpdated(EventArgs e)
	{
		Debug.Assert(IsMounted);
		Child.Layout = Build();
		base.OnLayoutUpdated(e);
	}

	[MemberNotNull(nameof(Child), nameof(View))]
	protected override void OnLayoutUpdating(EventArgs e)
	{
		base.OnLayoutUpdating(e);
		Debug.Assert(IsMounted);
	}

	[MemberNotNull(nameof(Child), nameof(View))]
	protected override void OnMounted(EventArgs e)
	{
		_child = Build().CreateElement();
		Debug.Assert(Child != null);
		Child.Mount(this);
		Debug.Assert(IsMounted);
		base.OnMounted(e);
	}

	protected override void OnUnmounted(EventArgs e)
	{
		Debug.Assert(Child != null);
		Child.Unmount();
		_child = null;
		base.OnUnmounted(e);
	}

	[MemberNotNull(nameof(Child), nameof(View))]
	protected override void OnUnmounting(EventArgs e)
	{
		base.OnUnmounting(e);
		Debug.Assert(IsMounted);
	}

	private Element? _child;
}
