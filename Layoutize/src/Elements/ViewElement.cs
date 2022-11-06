using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Layoutize.Layouts;
using Layoutize.Views;

namespace Layoutize.Elements;

internal abstract class ViewElement : Element
{
	public override IView? View => _view;

	public event EventHandler? Created;

	public event EventHandler? Creating;

	public event EventHandler? Deleted;

	public event EventHandler? Deleting;

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

	protected ViewElement(ViewLayout layout)
		: base(layout)
	{
	}

	[MemberNotNull(nameof(View))]
	protected override Action Mount()
	{
		Debug.Assert(!IsMounted);
		OnMounting(EventArgs.Empty);
		_view = Layout.CreateView(this);
		Build();
		if (!_view.Exists) Create();
		OnMounted(EventArgs.Empty);
		Debug.Assert(IsMounted);
		return () =>
		{
			Debug.Assert(IsMounted);
			OnUnmounting(EventArgs.Empty);
			if (Layout.DeleteOnUnmount && _view.Exists) Delete();
			Unbuild();
			_view = null;
			OnUnmounted(EventArgs.Empty);
			Debug.Assert(!IsMounted);
		};
	}

	[MemberNotNull(nameof(View))]
	protected virtual void OnCreated(EventArgs e)
	{
		Debug.Assert(View != null);
		Debug.Assert(View.Exists);
		Created?.Invoke(this, e);
	}

	[MemberNotNull(nameof(View))]
	protected virtual void OnCreating(EventArgs e)
	{
		Debug.Assert(View != null);
		Debug.Assert(!View.Exists);
		Creating?.Invoke(this, e);
	}

	[MemberNotNull(nameof(View))]
	protected virtual void OnDeleted(EventArgs e)
	{
		Debug.Assert(View != null);
		Debug.Assert(!View.Exists);
		Deleted?.Invoke(this, e);
	}

	[MemberNotNull(nameof(View))]
	protected virtual void OnDeleting(EventArgs e)
	{
		Debug.Assert(View != null);
		Debug.Assert(View.Exists);
		Deleting?.Invoke(this, e);
	}

	[MemberNotNullWhen(true, nameof(View))]
	public override bool IsMounted => base.IsMounted;

	private Action? Build()
	{
		Creating += Layout.OnCreating;
		Created += Layout.OnCreated;
		Deleting += Layout.OnDeleting;
		Deleted += Layout.OnDeleted;
		Mounting += Layout.OnMounting;
		Mounted += Layout.OnMounted;
		Unmounting += Layout.OnUnmounting;
		Unmounted += Layout.OnUnmounted;
		return () =>
		{
			Creating -= Layout.OnCreating;
			Created -= Layout.OnCreated;
			Deleting -= Layout.OnDeleting;
			Deleted -= Layout.OnDeleted;
			Mounting -= Layout.OnMounting;
			Mounted -= Layout.OnMounted;
			Unmounting -= Layout.OnUnmounting;
			Unmounted -= Layout.OnUnmounted;
		};
	}

	[MemberNotNull(nameof(View))]
	protected override void OnLayoutUpdated(EventArgs e)
	{
		Debug.Assert(IsMounted);
		Unbuild();
		Cleanup = Build();
		base.OnLayoutUpdated(e);
	}

	private void Unbuild()
	{
		Cleanup?.Invoke();
		Cleanup = null;
	}

	private Action? Cleanup { get; set; }

	[MemberNotNull(nameof(View))]
	private void Create()
	{
		Debug.Assert(View != null);
		Debug.Assert(!View.Exists);
		OnCreating(EventArgs.Empty);
		View.Create();
		OnCreated(EventArgs.Empty);
		Debug.Assert(View.Exists);
	}

	[MemberNotNull(nameof(View))]
	private void Delete()
	{
		Debug.Assert(View != null);
		Debug.Assert(View.Exists);
		OnDeleting(EventArgs.Empty);
		View.Delete();
		OnDeleted(EventArgs.Empty);
		Debug.Assert(!View.Exists);
	}

	private new ViewLayout Layout => (ViewLayout)base.Layout;

	private IView? _view;
}
