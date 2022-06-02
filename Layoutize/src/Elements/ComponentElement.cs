using System;
using System.Diagnostics;
using Layoutize.Layouts;
using Layoutize.Views;

namespace Layoutize.Elements;

internal abstract class ComponentElement : Element
{
	public Element Child
	{
		get => _child ?? throw new ElementNotMountedException(this);
		protected set
		{
			Debug.Assert(IsMounted);
			Child.Unmount();
			_child = value;
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
				Debug.Assert(_child != null);
				Debug.Assert(Child.IsMounted);
				Debug.Assert(Child.Parent == this);
				return true;
			}
			Debug.Assert(Parent == null);
			Debug.Assert(_child == null);
			return false;
		}
	}

	public override IView View => Child.View;

	protected ComponentElement(ComponentLayout layout)
		: base(layout)
	{
	}

	protected abstract Layout Build();

	protected override void OnLayoutUpdated(EventArgs e)
	{
		UpdateChild();
		base.OnLayoutUpdated(e);
	}

	protected override void OnMounting(EventArgs e)
	{
		base.OnMounting(e);
		_child = Build().CreateElement();
		Child.Mount(this);
	}

	protected override void OnUnmounted(EventArgs e)
	{
		Child.Unmount();
		_child = null;
		base.OnUnmounted(e);
	}

	private void UpdateChild()
	{
		Debug.Assert(IsMounted);
		Child.Layout = Build();
		Debug.Assert(IsMounted);
	}

	private Element? _child;
}
