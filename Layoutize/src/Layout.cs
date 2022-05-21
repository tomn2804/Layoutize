using Layoutize.Elements;
using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace Layoutize;

public abstract class Layout
{
    private protected Layout(IDictionary attributes)
    {
        Type thisType = GetType();
        foreach (DictionaryEntry attribute in attributes)
        {
            PropertyInfo property = thisType.GetProperty((string)attribute.Key)!;
            PropertyDescriptor? descriptor = TypeDescriptor.GetProperties(this)[property.Name];
            if (descriptor != null)
            {
                property.SetValue(this, descriptor.Converter.ConvertFrom(attribute.Value!));
            }
            else
            {
                property.SetValue(this, attribute.Value);
            }
        }
    }

    internal abstract Element CreateElement();
}
