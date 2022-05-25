using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Layoutize.Layouts;
using Layoutize.Views;

namespace Layoutize.Elements;

internal abstract class ComponentElement : Element
{
	public Element Child
	{
		get => _child.Value;
		protected set
		{
			Debug.Assert(IsMounted);
			OnChildUpdating(EventArgs.Empty);
			Child.Unmount();
			_child = new(() => value);
			Child.Mount(this);
			OnChildUpdated(EventArgs.Empty);
			Debug.Assert(IsMounted);
		}
	}

	[MemberNotNullWhen(true, nameof(Child))]
	public new bool IsMounted
	{
		get
		{
			if (base.IsMounted)
			{
				Debug.Assert(Parent != null);
				Debug.Assert(Child.IsMounted);
				Debug.Assert(Child.Parent == this);
				return true;
			}
			Debug.Assert(Parent == null);
			Debug.Assert(!Child.IsMounted);
			Debug.Assert(Child.Parent == null);
			return false;
		}
	}
	
	public override View View => Child.View;

	public event EventHandler? ChildUpdated;

	public event EventHandler? ChildUpdating;

	protected ComponentElement(ComponentLayout layout)
		: base(layout)
	{
		_child = new(() => Build().CreateElement());
	}

	protected abstract Layout Build();

	protected virtual void OnChildUpdated(EventArgs e)
	{
		Debug.Assert(IsMounted);
		ChildUpdated?.Invoke(this, e);
	}

	protected virtual void OnChildUpdating(EventArgs e)
	{
		Debug.Assert(IsMounted);
		ChildUpdating?.Invoke(this, e);
	}

	protected override void OnLayoutUpdated(EventArgs e)
	{
		Debug.Assert(IsMounted);
		Child.Layout = Build();
		Debug.Assert(IsMounted);
		base.OnLayoutUpdated(e);
	}

	protected override void OnMounting(EventArgs e)
	{
		base.OnMounting(e);
		Debug.Assert(!IsMounted);
		Child.Mount(this);
		Debug.Assert(IsMounted);
	}

	protected override void OnUnmounting(EventArgs e)
	{
		base.OnUnmounting(e);
		Debug.Assert(IsMounted);
		Child.Unmount();
		Debug.Assert(!IsMounted);
	}

	private Lazy<Element> _child;
}
