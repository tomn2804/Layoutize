using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Layoutize;

public abstract partial class ComponentElement : Element
{
    protected ComponentElement(Layout layout)
        : base(layout)
    {
    }

    protected abstract Layout Build();

    protected internal override void Mount(Element? parent)
    {
        Debug.Assert(!IsDisposed);
        base.Mount(parent);
        Child = Build().CreateElement();
        Child.Mount(this);
    }

    protected internal override void Unmount()
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(Child != null);
        Child.Unmount();
        Child = null;
    }

    protected override void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(Child != null);
        Child.Layout = Build();
        base.OnLayoutUpdated(e);
    }

    protected override void Dispose(bool disposing)
    {
        if (!IsDisposed && disposing)
        {
            Child?.Dispose();
        }
        base.Dispose(disposing);
    }
}

public abstract partial class ComponentElement
{
    public event EventHandler? ChildUpdating;

    public event EventHandler? ChildUpdated;

    private Element? _child;

    public Element? Child
    {
        get => _child;
        private protected set
        {
            Debug.Assert(!IsDisposed);
            OnChildUpdating(EventArgs.Empty);
            _child?.Dispose();
            _child = value;
            _child?.Mount(this);
            OnChildUpdated(EventArgs.Empty);
        }
    }

    protected virtual void OnChildUpdating(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        ChildUpdating?.Invoke(this, e);
    }

    protected virtual void OnChildUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        ChildUpdated?.Invoke(this, e);
    }
}

public sealed partial class StatelessElement : ComponentElement
{
    internal StatelessElement(StatelessLayout layout)
        : base(layout)
    {
    }

    private new StatelessLayout Layout => Layout;

    protected override Layout Build()
    {
        return Layout.Build(this);
    }
}

public sealed partial class StatefulElement : ComponentElement
{
    private new StatefulLayout Layout => Layout;

    internal StatefulElement(StatefulLayout layout)
        : base(layout)
    {
        State = Layout.CreateState();
        State.StateUpdating += UpdateChild;
    }

    private State State { get; }

    protected override void OnLayoutUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        State.Layout = Layout;
        base.OnLayoutUpdated(e);
    }

    private void UpdateChild(object? sender, EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        Debug.Assert(Child != null);
        Layout newChildLayout = Build();
        if (Child.Layout.GetType() == newChildLayout.GetType())
        {
            Child.Layout = newChildLayout;
        }
        else
        {
            Child = newChildLayout.CreateElement();
        }
    }

    protected override Layout Build()
    {
        return State.Build(this);
    }

    protected override void Dispose(bool disposing)
    {
        if (!IsDisposed && disposing)
        {
            State.Dispose();
        }
        base.Dispose(disposing);
    }
}

public class ScopeElement : ComponentElement
{
    private new ScopeLayout Layout => Layout;

    public override ImmutableDictionary<object, object> Scope => Layout.Attributes.SetItems(base.Scope);

    public ScopeElement(ScopeLayout layout)
        : base(layout)
    {
    }

    protected override Layout Build()
    {
        return (Layout)Layout.Attributes["Child"];
    }
}
