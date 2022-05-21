using Layoutize.Elements;
using Layoutize.Views;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Management.Automation;

namespace Layoutize;

public class EventTypeConverter : TypeConverter
{
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return value switch
        {
            ScriptBlock scriptBlock => new EventHandler((sender, e) => scriptBlock.Invoke(sender, e)),
            _ => base.ConvertFrom(context, culture, value),
        };
    }
}

public class NameTypeConverter : TypeConverter
{
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        string? name = value.ToString();
        if (name == null || string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidProgramException($"Attribute value 'Name' is null or contains only white spaces.");
        }
        if (name.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
        {
            throw new InvalidProgramException($"Attribute value 'Name' contains invalid characters.");
        }
        return name;
    }
}

public abstract class ViewLayout : Layout
{
    private protected ViewLayout(IDictionary attributes)
        : base(attributes)
    {
        Debug.Assert(Name != null);
    }

    [TypeConverter(typeof(TypeConverter))]
    public bool DeleteOnUnmount { get; }

    [TypeConverter(typeof(NameTypeConverter))]
    public string Name { get; }

    [TypeConverter(typeof(EventTypeConverter))]
    public EventHandler? OnCreated { get; }

    [TypeConverter(typeof(EventTypeConverter))]
    public EventHandler? OnCreating { get; }

    [TypeConverter(typeof(EventTypeConverter))]
    public EventHandler? OnDeleted { get; }

    [TypeConverter(typeof(EventTypeConverter))]
    public EventHandler? OnDeleting { get; }

    [TypeConverter(typeof(EventTypeConverter))]
    public EventHandler? OnMounted { get; }

    [TypeConverter(typeof(EventTypeConverter))]
    public EventHandler? OnMounting { get; }

    [TypeConverter(typeof(EventTypeConverter))]
    public EventHandler? OnUnmounted { get; }

    [TypeConverter(typeof(EventTypeConverter))]
    public EventHandler? OnUnmounting { get; }

    internal abstract override ViewElement CreateElement();

    internal abstract View CreateView(IBuildContext context);
}
