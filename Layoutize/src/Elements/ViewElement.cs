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
	}

	public new bool IsMounted
	{
		get
		{
			if (base.IsMounted)
			{
				Debug.Assert(_view != null);
				return true;
			}
			return false;
		}
	}

	protected virtual void OnCreated(EventArgs e)
	{
		Created?.Invoke(this, e);
	}

	protected virtual void OnCreating(EventArgs e)
	{
		Creating?.Invoke(this, e);
	}

	protected virtual void OnDeleted(EventArgs e)
	{
		Deleted?.Invoke(this, e);
	}

	protected virtual void OnDeleting(EventArgs e)
	{
		Deleting?.Invoke(this, e);
	}

	protected override void OnLayoutUpdated(EventArgs e)
	{
		RemoveEventHandler();
		base.OnLayoutUpdated(e);
	}

	protected override void OnLayoutUpdating(EventArgs e)
	{
		base.OnLayoutUpdating(e);
		RemoveEventHandler();
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
		Debug.Assert(!View.Exists);
		OnCreating(EventArgs.Empty);
		View.Create();
		OnCreated(EventArgs.Empty);
		Debug.Assert(View.Exists);
	}

	private void Delete()
	{
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

	private IView? _view;
}
