using Layoutize.Elements;
using System;
using System.Collections;
using System.Diagnostics;

namespace Layoutize;

public abstract partial class State
{
    private StatefulElement? _element;

    internal StatefulElement Element
    {
        get
        {
            Debug.Assert(_element != null);
            return _element;
        }
        set => _element = value;
    }

    protected internal abstract Layout Build(IBuildContext context);
}

public abstract partial class State
{
    internal event EventHandler? StateUpdated;

    internal event EventHandler? StateUpdating;

    private protected virtual void OnStateUpdated(EventArgs e)
    {
        StateUpdated?.Invoke(this, e);
    }

    private protected virtual void OnStateUpdating(EventArgs e)
    {
        StateUpdating?.Invoke(this, e);
    }

    protected void SetState(IDictionary properties)
    {
        OnStateUpdating(EventArgs.Empty);
        foreach (DictionaryEntry property in properties)
        {
            GetType().GetProperty((string)property.Key)!.SetValue(this, property.Value);
        }
        OnStateUpdated(EventArgs.Empty);
    }
}

public abstract class State<T> : State where T : StatefulLayout
{
    protected T Layout => (T)Element.Layout;
}
