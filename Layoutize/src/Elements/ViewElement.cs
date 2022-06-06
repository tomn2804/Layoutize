using System;
using System.Diagnostics;
using Layoutize.Layouts;
using Layoutize.Views;

namespace Layoutize.Elements;

internal abstract class ViewElement : Element
{
	public override IView View => _view.Value;

	public event EventHandler? Created;

	public event EventHandler? Creating;

	public event EventHandler? Deleted;

	public event EventHandler? Deleting;

	protected ViewElement(Element? parent, ViewLayout layout)
		: base(parent, layout)
	{
		_view = new(() => Layout.CreateView(this));
		Build();
	}

	protected virtual void OnCreated(EventArgs e)
	{
		Debug.Assert(View.Exists);
		Created?.Invoke(this, e);
		Debug.Assert(View.Exists);
	}

	protected virtual void OnCreating(EventArgs e)
	{
		Debug.Assert(!View.Exists);
		Creating?.Invoke(this, e);
		Debug.Assert(!View.Exists);
	}

	protected virtual void OnDeleted(EventArgs e)
	{
		Debug.Assert(!View.Exists);
		Deleted?.Invoke(this, e);
		Debug.Assert(!View.Exists);
	}

	protected virtual void OnDeleting(EventArgs e)
	{
		Debug.Assert(View.Exists);
		Deleting?.Invoke(this, e);
		Debug.Assert(View.Exists);
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
		if (!View.Exists) Create();
		Debug.Assert(!IsMounted);
	}

	protected override void OnUnmounted(EventArgs e)
	{
		Debug.Assert(!IsMounted);
		if (Layout.DeleteOnUnmount && View.Exists) Delete();
		_view = new(() => Layout.CreateView(this));
		base.OnUnmounted(e);
	}

	private void Build()
	{
		Rebuild();
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

	private Lazy<IView> _view;
}
