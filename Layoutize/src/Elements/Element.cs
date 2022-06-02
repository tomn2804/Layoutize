using System;
using System.ComponentModel.DataAnnotations;
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

	public bool IsMounted
	{
		get
		{
			if (_isMounted) Debug.Assert(View.Exists);
			return _isMounted;
		}
	}

	public Layout Layout
	{
		get
		{
			Debug.Assert(Validator.TryValidateObject(_layout, new(_layout), null));
			return _layout;
		}
		set
		{
			Debug.Assert(IsMounted);
			Validator.ValidateObject(value, new(value));
			OnLayoutUpdating(EventArgs.Empty);
			_layout = value;
			OnLayoutUpdated(EventArgs.Empty);
			Debug.Assert(IsMounted);
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
		Validator.ValidateObject(layout, new(layout));
		_layout = layout;
	}

	protected virtual void OnLayoutUpdated(EventArgs e)
	{
		LayoutUpdated?.Invoke(this, e);
	}

	protected virtual void OnLayoutUpdating(EventArgs e)
	{
		LayoutUpdating?.Invoke(this, e);
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

	Element IBuildContext.Element => this;

	private bool _isMounted;

	private Layout _layout;
}
