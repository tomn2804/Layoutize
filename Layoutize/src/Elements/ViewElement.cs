using System;
using System.Diagnostics;
using Layoutize.Layouts;
using Layoutize.Views;

namespace Layoutize.Elements;

internal abstract class ViewElement : Element
{
	public override IView View => _view ?? throw new ElementNotMountedException(this);

	public event EventHandler? Created;

	public event EventHandler? Creating;

	public event EventHandler? Deleted;

	public event EventHandler? Deleting;

	protected ViewElement(ViewLayout layout)
		: base(layout)
	{
		AddEventHandler();
		ViewModel.LayoutUpdating += (sender, e) => RemoveEventHandler();
		ViewModel.LayoutUpdated += (sender, e) => AddEventHandler();
	}

	protected virtual void OnCreated(EventArgs e)
	{
		Debug.Assert(View != null);
		Debug.Assert(View.Exists);
		Created?.Invoke(this, e);
		Debug.Assert(View.Exists);
	}

	protected virtual void OnCreating(EventArgs e)
	{
		Debug.Assert(View != null);
		Debug.Assert(!View.Exists);
		Creating?.Invoke(this, e);
		Debug.Assert(!View.Exists);
	}

	protected virtual void OnDeleted(EventArgs e)
	{
		Debug.Assert(View != null);
		Debug.Assert(!View.Exists);
		Deleted?.Invoke(this, e);
		Debug.Assert(!View.Exists);
	}

	protected virtual void OnDeleting(EventArgs e)
	{
		Debug.Assert(View != null);
		Debug.Assert(View.Exists);
		Deleting?.Invoke(this, e);
		Debug.Assert(View.Exists);
	}

	protected override void OnMounting(EventArgs e)
	{
		base.OnMounting(e);
		_view = Layout.CreateView(this);
		if (!View.Exists) Create();
	}

	protected override void OnUnmounted(EventArgs e)
	{
		if (Layout.DeleteOnUnmount && View.Exists) Delete();
		_view = null;
		base.OnUnmounted(e);
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
		OnCreating(EventArgs.Empty);
		View.Create();
		OnCreated(EventArgs.Empty);
	}

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

	private IView? _view;
}
