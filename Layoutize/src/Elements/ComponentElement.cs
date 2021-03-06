using System;
using System.Diagnostics;
using Layoutize.Layouts;
using Layoutize.Views;

namespace Layoutize.Elements;

internal abstract class ComponentElement : Element
{
	public Element Child
	{
		get
		{
			var child = _child.Value;
			Debug.Assert(child.Parent == this);
			return child;
		}
		protected set
		{
			Debug.Assert(!value.IsMounted);
			Debug.Assert(value.Parent == this);
			Debug.Assert(IsMounted);
			Child.Unmount();
			_child = new(() => value);
			Child.Mount();
			Debug.Assert(IsMounted);
		}
	}

	public new bool IsMounted
	{
		get
		{
			var isMounted = base.IsMounted;
			if (isMounted)
			{
				Debug.Assert(Child.IsMounted);
				Debug.Assert(Child.Parent == this);
			}
			return isMounted;
		}
	}

	public override IView View => Child.View;

	protected ComponentElement(Element parent, ComponentLayout layout)
		: base(parent, layout)
	{
		_child = new(() => Build().CreateElement(this));
	}

	protected abstract Layout Build();

	protected override void OnLayoutUpdated(EventArgs e)
	{
		Debug.Assert(IsMounted);
		Rebuild();
		base.OnLayoutUpdated(e);
	}

	protected override void OnMounting(EventArgs e)
	{
		base.OnMounting(e);
		Child.Mount();
		Debug.Assert(!IsMounted);
	}

	protected override void OnUnmounted(EventArgs e)
	{
		Debug.Assert(!IsMounted);
		Child.Unmount();
		_child = new(() => Build().CreateElement(this));
		base.OnUnmounted(e);
	}

	private void Rebuild()
	{
		Debug.Assert(IsMounted);
		Child.Layout = Build();
		Debug.Assert(IsMounted);
	}

	private Lazy<Element> _child;
}
