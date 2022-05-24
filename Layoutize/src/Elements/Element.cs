using System;
using System.Diagnostics;
using Layoutize.Contexts;
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

	public abstract View View { get; }

	public bool IsMounted
	{
		get
		{
			if (!_isMounted) return false;
			Debug.Assert(View.Exists);
			return true;
		}
	}

	public Layout Layout
	{
		get => _layout;
		set
		{
			Debug.Assert(IsMounted);
			OnLayoutUpdating(EventArgs.Empty);
			_layout = value;
			OnLayoutUpdated(EventArgs.Empty);
		}
	}

	public Element? Parent { get; private set; }

	public event EventHandler? LayoutUpdated;

	public event EventHandler? LayoutUpdating;

	public event EventHandler? Mounted;

	public event EventHandler? Mounting;

	public event EventHandler? Unmounted;

	public event EventHandler? Unmounting;

	protected Element(Layout layout)
	{
		_layout = layout;
	}

	protected virtual void OnLayoutUpdated(EventArgs e)
	{
		Debug.Assert(IsMounted);
		LayoutUpdated?.Invoke(this, e);
	}

	protected virtual void OnLayoutUpdating(EventArgs e)
	{
		Debug.Assert(IsMounted);
		LayoutUpdating?.Invoke(this, e);
	}

	protected virtual void OnMounted(EventArgs e)
	{
		Debug.Assert(IsMounted);
		Mounted?.Invoke(this, e);
	}

	protected virtual void OnMounting(EventArgs e)
	{
		Debug.Assert(!IsMounted);
		Mounting?.Invoke(this, e);
	}

	protected virtual void OnUnmounted(EventArgs e)
	{
		Debug.Assert(!IsMounted);
		Unmounted?.Invoke(this, e);
	}

	protected virtual void OnUnmounting(EventArgs e)
	{
		Debug.Assert(IsMounted);
		Unmounting?.Invoke(this, e);
	}

	Element IBuildContext.Element => this;

	private bool _isMounted;

	private Layout _layout;
}
