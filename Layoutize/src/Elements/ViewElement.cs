using System;
using System.Diagnostics;
using Layoutize.Layouts;
using Layoutize.Views;

namespace Layoutize.Elements;

internal abstract class ViewElement : Element
{
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

	protected virtual void OnCreated(EventArgs e)
	{
		Debug.Assert(View != null);
		Debug.Assert(View.Exists);
		Created?.Invoke(this, e);
	}

	protected virtual void OnCreating(EventArgs e)
	{
		Debug.Assert(View != null);
		Debug.Assert(!View.Exists);
		Creating?.Invoke(this, e);
	}

	protected virtual void OnDeleted(EventArgs e)
	{
		Debug.Assert(View != null);
		Debug.Assert(!View.Exists);
		Deleted?.Invoke(this, e);
	}

	protected virtual void OnDeleting(EventArgs e)
	{
		Debug.Assert(View != null);
		Debug.Assert(View.Exists);
		Deleting?.Invoke(this, e);
	}

	protected override void OnLayoutUpdated(EventArgs e)
	{
		Debug.Assert(IsMounted);
		AddEventHandler();
		base.OnLayoutUpdated(e);
	}

	protected override void OnLayoutUpdating(EventArgs e)
	{
		base.OnLayoutUpdating(e);
		Debug.Assert(IsMounted);
		RemoveEventHandler();
	}

	protected override void OnMounting(EventArgs e)
	{
		base.OnMounting(e);
		Debug.Assert(!IsMounted);
		_view = Layout.CreateView(this);
		if (!_view.Exists) Create();
	}

	protected override void OnUnmounting(EventArgs e)
	{
		base.OnUnmounting(e);
		Debug.Assert(IsMounted);
		Debug.Assert(View != null);
		if (Layout.DeleteOnUnmount && View.Exists) Delete();
		_view = null;
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

	private void Create()
	{
		Debug.Assert(View != null);
		Debug.Assert(!View.Exists);
		OnCreating(EventArgs.Empty);
		View.Create();
		OnCreated(EventArgs.Empty);
		Debug.Assert(View.Exists);
	}

	private void Delete()
	{
		Debug.Assert(View != null);
		Debug.Assert(View.Exists);
		OnDeleting(EventArgs.Empty);
		View.Delete();
		OnDeleted(EventArgs.Empty);
		Debug.Assert(!View.Exists);
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
