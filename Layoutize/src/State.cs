using Layoutize.Elements;
using System;
using System.Collections;
using System.Reflection;

namespace Layoutize;

public abstract partial class State
{
    protected State(StatefulLayout layout)
    {
        Layout = layout;
    }

    internal StatefulLayout Layout
    {
        set
        {
            Type thisType = GetType();
            foreach (PropertyInfo attribute in value.GetType().GetProperties())
            {
                thisType.GetProperty(attribute.Name)!.SetValue(this, attribute.GetValue(value));
            }
        }
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
