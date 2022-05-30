using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Layoutize.Layouts;
using Layoutize.Views;

namespace Layoutize.Elements;

internal abstract class ViewElement : Element
{
	[MemberNotNull(nameof(View))]
	public override void Mount(Element? parent)
	{
		base.Mount(parent);
		Debug.Assert(IsMounted);
	}

	[MemberNotNullWhen(true, nameof(View))]
	public override bool IsMounted => base.IsMounted;

	[NotNullIfNotNull(nameof(_view))]
	public override View? View => _view;

	public event EventHandler? Created;

	public event EventHandler? Creating;

	public event EventHandler? Deleted;

	public event EventHandler? Deleting;

	protected ViewElement(ViewLayout layout)
		: base(layout)
	{
		AddEventHandler();
	}

	[MemberNotNull(nameof(View))]
	protected virtual void OnCreated(EventArgs e)
	{
		Debug.Assert(View != null);
		Debug.Assert(View.Exists);
		Created?.Invoke(this, e);
		Debug.Assert(View.Exists);
	}

	[MemberNotNull(nameof(View))]
	protected virtual void OnCreating(EventArgs e)
	{
		Debug.Assert(View != null);
		Debug.Assert(!View.Exists);
		Creating?.Invoke(this, e);
		Debug.Assert(!View.Exists);
	}

	[MemberNotNull(nameof(View))]
	protected virtual void OnDeleted(EventArgs e)
	{
		Debug.Assert(View != null);
		Debug.Assert(!View.Exists);
		Deleted?.Invoke(this, e);
		Debug.Assert(!View.Exists);
	}

	[MemberNotNull(nameof(View))]
	protected virtual void OnDeleting(EventArgs e)
	{
		Debug.Assert(View != null);
		Debug.Assert(View.Exists);
		Deleting?.Invoke(this, e);
		Debug.Assert(View.Exists);
	}

	[MemberNotNull(nameof(View))]
	protected override void OnLayoutUpdated(EventArgs e)
	{
		Debug.Assert(IsMounted);
		AddEventHandler();
		base.OnLayoutUpdated(e);
	}

	[MemberNotNull(nameof(View))]
	protected override void OnLayoutUpdating(EventArgs e)
	{
		base.OnLayoutUpdating(e);
		RemoveEventHandler();
		Debug.Assert(IsMounted);
	}

	[MemberNotNull(nameof(View))]
	protected override void OnMounted(EventArgs e)
	{
		Debug.Assert(IsMounted);
		base.OnMounted(e);
	}

	[MemberNotNull(nameof(View))]
	protected override void OnMounting(EventArgs e)
	{
		base.OnMounting(e);
		_view = Layout.CreateView(this);
		Debug.Assert(View != null);
		if (!View.Exists) Create();
	}

	protected override void OnUnmounted(EventArgs e)
	{
		Debug.Assert(View != null);
		if (Layout.DeleteOnUnmount && View.Exists) Delete();
		_view = null;
		base.OnUnmounted(e);
	}

	[MemberNotNull(nameof(View))]
	protected override void OnUnmounting(EventArgs e)
	{
		base.OnUnmounting(e);
		Debug.Assert(IsMounted);
	}

	private void AddEventHandler()
	{
		Creating += Layout.OnCreating;
		Created += Layout.OnCreated;
		Deleting += Layout.OnDeleting;
		Deleted += Layout.OnDeleted;
		Mounting += Layout.OnMounting;
		Mounted += Layout.OnMounted;
		Unmounting += Layout.OnUnmounting;
		Unmounted += Layout.OnUnmounted;
	}

	[MemberNotNull(nameof(View))]
	private void Create()
	{
		OnCreating(EventArgs.Empty);
		View.Create();
		OnCreated(EventArgs.Empty);
	}

	[MemberNotNull(nameof(View))]
	private void Delete()
	{
		OnDeleting(EventArgs.Empty);
		View.Delete();
		OnDeleted(EventArgs.Empty);
	}

	private void RemoveEventHandler()
	{
		Creating -= Layout.OnCreating;
		Created -= Layout.OnCreated;
		Deleting -= Layout.OnDeleting;
		Deleted -= Layout.OnDeleted;
		Mounting -= Layout.OnMounting;
		Mounted -= Layout.OnMounted;
		Unmounting -= Layout.OnUnmounting;
		Unmounted -= Layout.OnUnmounted;
	}

	private new ViewLayout Layout => (ViewLayout)base.Layout;

	private View? _view;
}
