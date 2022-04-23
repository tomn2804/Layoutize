using System;
using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Layoutize;

public abstract partial class State
{
    internal StatefulLayout Layout { get; set; }

    protected State(StatefulLayout layout)
    {
        Layout = layout;
    }

    public ImmutableDictionary<object, object> Attributes => Layout.Attributes;

    protected internal abstract Layout Build(IBuildContext context);
}

public abstract partial class State
{
    internal event EventHandler? StateUpdated;

    internal event EventHandler? StateUpdating;

    private protected virtual void OnStateUpdated(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        StateUpdated?.Invoke(this, e);
    }

    private protected virtual void OnStateUpdating(EventArgs e)
    {
        Debug.Assert(!IsDisposed);
        StateUpdating?.Invoke(this, e);
    }

    protected void SetState(IDictionary properties)
    {
        Debug.Assert(!IsDisposed);
        OnStateUpdating(EventArgs.Empty);
        Type @this = GetType();
        foreach (DictionaryEntry property in properties)
        {
            @this.GetProperty((string)property.Key)!.SetValue(this, property.Value);
        }
        OnStateUpdated(EventArgs.Empty);
    }
}

public abstract partial class State : IDisposable
{
    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                StateUpdated = null;
                StateUpdating = null;
            }
            IsDisposed = true;
        }
    }
}
