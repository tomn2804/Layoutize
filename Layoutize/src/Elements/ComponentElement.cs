using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Layoutize.Layouts;
using Layoutize.Views;

namespace Layoutize.Elements;

internal abstract class ComponentElement : Element
{
	[DisallowNull]
	public Element? Child
	{
		get => _child;
		protected set
		{
			Debug.Assert(IsMounted);
			OnChildUpdating(EventArgs.Empty);
			_child.Unmount();
			_child = value;
			_child.Mount(this);
			OnChildUpdated(EventArgs.Empty);
			Debug.Assert(IsMounted);
		}
	}

	[MemberNotNullWhen(true, nameof(Child), nameof(_child))]
	public new bool IsMounted
	{
		get
		{
			if (base.IsMounted)
			{
				Debug.Assert(Parent != null);
				Debug.Assert(_child != null);
				Debug.Assert(Child != null);
				Debug.Assert(Child.IsMounted);
				Debug.Assert(Child.Parent == this);
				return true;
			}
			Debug.Assert(Parent == null && Child == null);
			return false;
		}
	}

	public override View? View => Child?.View;

	public event EventHandler? ChildUpdated;

	public event EventHandler? ChildUpdating;

	protected ComponentElement(ComponentLayout layout)
		: base(layout)
	{
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
		UpdateChild();
		base.OnLayoutUpdated(e);
	}

	protected override void OnMounting(EventArgs e)
	{
		base.OnMounting(e);
		Debug.Assert(!IsMounted);
		_child = Build().CreateElement();
		_child.Mount(this);
		Debug.Assert(IsMounted);
	}

	protected override void OnUnmounting(EventArgs e)
	{
		base.OnUnmounting(e);
		Debug.Assert(IsMounted);
		_child.Unmount();
		_child = null;
		Debug.Assert(!IsMounted);
	}

	private void UpdateChild()
	{
		Debug.Assert(IsMounted);
		Child.Layout = Build();
	}

	private Element? _child;
}
