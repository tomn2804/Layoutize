using Layoutize.Elements;
using System;
using System.Reflection;

namespace Layoutize;

public abstract class Layout
{
    public Layout Inherit
    {
        init
        {
            Type thisType = GetType();
            foreach (PropertyInfo property in value.GetType().GetProperties())
            {
                thisType.GetProperty(property.Name)?.SetValue(this, property.GetValue(value));
            }
        }
    }

    internal abstract Element CreateElement();
}
