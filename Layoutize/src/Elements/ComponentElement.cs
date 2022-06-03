using System;
using System.Diagnostics;
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
			Child.Unmount();
			_child = new(() => value);
			Child.Mount(this);
			Debug.Assert(IsMounted);
		}
	}

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
			return false;
		}
	}

	public override IView View => Child.View;

	protected ComponentElement(ComponentLayout layout)
		: base(layout)
	{
		_child = new(() => Layout.CreateElement());
	}

	protected abstract Layout Build();

	protected override void OnLayoutUpdated(EventArgs e)
	{
		Debug.Assert(IsMounted);
		UpdateChild();
		base.OnLayoutUpdated(e);
	}

	protected override void OnMounting(EventArgs e)
	{
		base.OnMounting(e);
		Child.Mount(this);
		Debug.Assert(!IsMounted);
	}

	protected override void OnUnmounted(EventArgs e)
	{
		Debug.Assert(!IsMounted);
		Child.Unmount();
		_child = new(() => Layout.CreateElement());
		base.OnUnmounted(e);
	}

	private void UpdateChild()
	{
		Debug.Assert(IsMounted);
		Child.Layout = Build();
		Debug.Assert(IsMounted);
	}

	private Lazy<Element> _child;
}
