using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Layoutize.Layouts;
using Layoutize.Views;

namespace Layoutize.Elements;

internal abstract class ComponentElement : Element
{
	[MemberNotNullWhen(true, nameof(Child))]
	public override bool IsMounted
	{
		get
		{
			if (base.IsMounted)
			{
				Debug.Assert(Child != null);
				return true;
			}
			Debug.Assert(Child == null);
			return false;
		}
	}

	[DisallowNull]
	public Element? Child
	{
		get
		{
			if (_child != null)
			{
				Debug.Assert(_child.IsMounted);
				Debug.Assert(_child.Parent == this);
			}
			return _child;
		}
		protected set
		{
			Debug.Assert(!value.IsMounted);
			Debug.Assert(value.Parent == null);
			Debug.Assert(IsMounted);
			Child.Unmount();
			_child = value;
			Child.MountTo(this);
			Debug.Assert(Child == value);
		}
	}

	public override IView? View => Child?.View;

	protected ComponentElement(ComponentLayout layout)
		: base(layout)
	{
	}

	protected abstract Layout Build();

	[MemberNotNull(nameof(Child))]
	protected override Action? Mount()
	{
		Debug.Assert(!IsMounted);
		_child = Build().CreateElement();
		_child.MountTo(this);
		Debug.Assert(Child != null);
		return () =>
		{
			Child.Unmount();
			_child = null;
			Debug.Assert(Child == null);
		};
	}

	[MemberNotNull(nameof(Child))]
	protected override void OnLayoutUpdated(EventArgs e)
	{
		Debug.Assert(IsMounted);
		Child.Layout = Build();
		base.OnLayoutUpdated(e);
	}

	private Element? _child;
}
