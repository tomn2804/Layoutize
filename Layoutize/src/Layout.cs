using Layoutize.Elements;
using System.Reflection;

namespace Layoutize;

public abstract class Layout
{
    public Layout Inherit
    {
        init
        {
            foreach (PropertyInfo property in value.GetType().GetProperties())
            {
                GetType().GetProperty(property.Name)?.SetValue(this, property.GetValue(value));
            }
        }
    }

    internal abstract Element CreateElement();
}
