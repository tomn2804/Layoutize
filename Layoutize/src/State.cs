using Layoutize.Elements;
using System;
using System.Collections;
using System.Diagnostics;

namespace Layoutize;

public abstract partial class State<T> where T : StatefulLayout<T>
{
    private StatefulElement<T>? _element;

    internal StatefulElement<T> Element
    {
        get
        {
            Debug.Assert(_element != null);
            return _element;
        }
        set => _element = value;
    }

    protected T Layout => Element.Layout;

    protected internal abstract Layout Build(IBuildContext context);
}

public abstract partial class State<T>
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
