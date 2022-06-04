using System;
using System.Diagnostics;
using Layoutize.Layouts;
using Layoutize.Views;

namespace Layoutize.Elements;

internal abstract class ViewElement : Element
{
	public override IViewContext? ViewContext => _view;

	public event EventHandler? Created;

	public event EventHandler? Creating;

	public event EventHandler? Deleted;

	public event EventHandler? Deleting;

	protected ViewElement(ViewLayout layout)
		: base(layout)
	{
		Build();
	}

	protected virtual void OnCreated(EventArgs e)
	{
		Debug.Assert(_view != null);
		Debug.Assert(_view.Exists);
		Created?.Invoke(this, e);
		Debug.Assert(_view.Exists);
	}

	protected virtual void OnCreating(EventArgs e)
	{
		Debug.Assert(_view != null);
		Debug.Assert(!_view.Exists);
		Creating?.Invoke(this, e);
		Debug.Assert(!_view.Exists);
	}

	protected virtual void OnDeleted(EventArgs e)
	{
		Debug.Assert(_view != null);
		Debug.Assert(!_view.Exists);
		Deleted?.Invoke(this, e);
		Debug.Assert(!_view.Exists);
	}

	protected virtual void OnDeleting(EventArgs e)
	{
		Debug.Assert(_view != null);
		Debug.Assert(_view.Exists);
		Deleting?.Invoke(this, e);
		Debug.Assert(_view.Exists);
	}

	protected override void OnLayoutUpdated(EventArgs e)
	{
		Debug.Assert(IsMounted);
		Rebuild();
		base.OnLayoutUpdated(e);
	}

	protected override void OnLayoutUpdating(EventArgs e)
	{
		base.OnLayoutUpdating(e);
		Unbuild();
		Debug.Assert(IsMounted);
	}

	protected override void OnMounting(EventArgs e)
	{
		base.OnMounting(e);
		_view = Layout.CreateView(this);
		if (!_view.Exists) Create();
		Debug.Assert(!IsMounted);
	}

	protected override void OnUnmounted(EventArgs e)
	{
		Debug.Assert(!IsMounted);
		Debug.Assert(_view != null);
		if (Layout.DeleteOnUnmount && _view.Exists) Delete();
		_view = null;
		base.OnUnmounted(e);
	}

	private void Build()
	{
		Rebuild();
	}

	private void Create()
	{
		Debug.Assert(_view != null);
		Debug.Assert(!_view.Exists);
		OnCreating(EventArgs.Empty);
		_view.Create();
		OnCreated(EventArgs.Empty);
		Debug.Assert(_view.Exists);
	}

	private void Delete()
	{
		Debug.Assert(_view != null);
		Debug.Assert(_view.Exists);
		OnDeleting(EventArgs.Empty);
		_view.Delete();
		OnDeleted(EventArgs.Empty);
		Debug.Assert(!_view.Exists);
	}

	private void Rebuild()
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

	private void Unbuild()
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
