using System;
using System.Diagnostics;
using Layoutize.Contexts;
using Layoutize.Layouts;
using Layoutize.Views;

namespace Layoutize.Elements;

internal abstract class Element : IBuildContext, IComparable<Element>
{
	public int CompareTo(Element? other)
	{
		return other == null ? 1 : string.Compare(Name.Of(this), Name.Of(other), StringComparison.Ordinal);
	}

	public void Mount(Element? parent)
	{
		Debug.Assert(!IsMounted);
		Parent = parent;
		OnMounting(EventArgs.Empty);
		_isMounted = true;
		OnMounted(EventArgs.Empty);
		Debug.Assert(IsMounted);
	}

	public void Unmount()
	{
		Debug.Assert(IsMounted);
		OnUnmounting(EventArgs.Empty);
		_isMounted = false;
		OnUnmounted(EventArgs.Empty);
		Parent = null;
		Debug.Assert(!IsMounted);
	}

	public abstract IView View { get; }

	public virtual bool IsMounted
	{
		get
		{
			if (_isMounted) Debug.Assert(View.Exists);
			return _isMounted;
		}
	}

	public Layout Layout
	{
		get => ViewModel.Layout;
		set
		{
			Debug.Assert(IsMounted);
			ViewModel.Layout = value;
			Debug.Assert(IsMounted);
		}
	}

	public Element? Parent { get; private set; }

	public event EventHandler? Mounted;

	public event EventHandler? Mounting;

	public event EventHandler? Unmounted;

	public event EventHandler? Unmounting;

	protected Element(Layout layout)
	{
		ViewModel = new(layout);
	}

	protected virtual void OnMounted(EventArgs e)
	{
		Mounted?.Invoke(this, e);
	}

	protected virtual void OnMounting(EventArgs e)
	{
		Mounting?.Invoke(this, e);
	}

	protected virtual void OnUnmounted(EventArgs e)
	{
		Unmounted?.Invoke(this, e);
	}

	protected virtual void OnUnmounting(EventArgs e)
	{
		Unmounting?.Invoke(this, e);
	}

	protected readonly ViewModel ViewModel;

	Element IBuildContext.Element => this;

	private bool _isMounted;
}
