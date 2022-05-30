using System;
using System.Diagnostics;
using Layoutize.Layouts;
using Layoutize.Views;

namespace Layoutize.Elements;

internal abstract class ComponentElement : Element
{
	public Element Child
	{
		get => _child ?? throw new ElementNotMountedException();
		protected set
		{
			Debug.Assert(IsMounted);
			Child.Unmount();
			_child = value;
			Child.Mount(this);
			Debug.Assert(IsMounted);
		}
	}

	public override bool IsMounted
	{
		get
		{
			var result = base.IsMounted;
			if (result)
			{
				Debug.Assert(Parent != null);
				Debug.Assert(Child.IsMounted);
				Debug.Assert(Child.Parent == this);
			}
			else
			{
				Debug.Assert(Parent == null);
			}
			return result;
		}
	}

	public override View View => Child.View;

	protected ComponentElement(ComponentLayout layout)
		: base(layout)
	{
		ViewModel.LayoutUpdated += (sender, e) => UpdateChild();
	}

	protected abstract Layout Build();

	private void UpdateChild()
	{
		Debug.Assert(IsMounted);
		Child.Layout = Build();
	}

	protected override void OnMounting(EventArgs e)
	{
		base.OnMounting(e);
		_child = Build().CreateElement();
		Child.Mount(this);
		Debug.Assert(IsMounted);
	}

	protected override void OnUnmounted(EventArgs e)
	{
		Child.Unmount();
		_child = null;
		base.OnUnmounted(e);
	}

	private Element? _child;
}
