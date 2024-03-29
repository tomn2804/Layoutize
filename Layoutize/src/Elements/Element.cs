﻿using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Layoutize.Annotations;
using Layoutize.Layouts;
using Layoutize.Views;

namespace Layoutize.Elements;

internal abstract class Element : IBuildContext, IComparable<Element>
{
	public int CompareTo(Element? other)
	{
		return string.Compare(Layout.Name, other != null ? other.Layout.Name : null, StringComparison.Ordinal);
	}

	public event EventHandler? Mounted;

	public event EventHandler? Mounting;

	public event EventHandler? Unmounted;

	public event EventHandler? Unmounting;

	[MemberNotNull(nameof(View))]
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

	[MemberNotNull(nameof(View))]
	protected virtual void OnUnmounting(EventArgs e)
	{
		Debug.Assert(IsMounted);
		Unmounting?.Invoke(this, e);
	}

	[MemberNotNull(nameof(View))]
	public void MountTo(Element? parent)
	{
		Debug.Assert(!IsMounted);
		OnMounting(EventArgs.Empty);
		Parent = parent;
		_layout.InitState(this);
		Cleanup = Mount();
		OnMounted(EventArgs.Empty);
		Debug.Assert(IsMounted);
	}

	protected abstract Action? Mount();

	public void Unmount()
	{
		Debug.Assert(IsMounted);
		OnUnmounting(EventArgs.Empty);
		Cleanup?.Invoke();
		Cleanup = null;
		Parent = null;
		OnUnmounted(EventArgs.Empty);
		Debug.Assert(!IsMounted);
	}

	private Action? Cleanup { get; set; }

	public abstract IView? View { get; }

	[MemberNotNullWhen(true, nameof(Parent), nameof(View))]
	public virtual bool IsMounted
	{
		get
		{
			if (View != null)
			{
				Debug.Assert(Parent != null);
				Debug.Assert(View.Exists);
				return true;
			}
			Debug.Assert(Parent == null);
			return false;
		}
	}

	public Layout Layout
	{
		get
		{
			Debug.Assert(_layout.IsValid());
			return _layout;
		}
		[MemberNotNull(nameof(Parent), nameof(View))]
		set
		{
			Debug.Assert(value.IsValid());
			Debug.Assert(IsMounted);
			OnLayoutUpdating(EventArgs.Empty);
			_layout = value;
			OnLayoutUpdated(EventArgs.Empty);
			Debug.Assert(Layout == value);
		}
	}

	public virtual Element? Parent { get; private set; }

	public event EventHandler? LayoutUpdated;

	public event EventHandler? LayoutUpdating;

	protected Element(Layout layout)
	{
		_layout = layout;
		Debug.Assert(!IsMounted);
	}

	[MemberNotNull(nameof(Parent), nameof(View))]
	protected virtual void OnLayoutUpdated(EventArgs e)
	{
		Debug.Assert(IsMounted);
		LayoutUpdated?.Invoke(this, e);
	}

	[MemberNotNull(nameof(Parent), nameof(View))]
	protected virtual void OnLayoutUpdating(EventArgs e)
	{
		Debug.Assert(IsMounted);
		LayoutUpdating?.Invoke(this, e);
	}

	Element IBuildContext.Element => this;

	private Layout _layout;
}
