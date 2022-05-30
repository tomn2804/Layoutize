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

	public override IView View => Child.View;

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
		Debug.Assert(IsMounted);
	}
	
	protected override void OnMounted(EventArgs e)
	{
		_child = Build().CreateElement();
		Child.Mount(this);
		base.OnMounted(e);
	}

	protected override void OnUnmounting(EventArgs e)
	{
		base.OnUnmounting(e);
		Child.Unmount();
		_child = null;
	}

	private Element? _child;
}
