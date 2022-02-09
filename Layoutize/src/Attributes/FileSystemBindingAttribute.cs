using System;
using Templatize.Views;

namespace Templatize.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class FileSystemBindingAttribute : Attribute
{
    public static readonly Type Name = typeof(string);

    public static readonly Type OnCreated = typeof(InvokedEventArgs);
    public static readonly Type OnCreating = typeof(InvokingEventArgs);

    public static readonly Type OnMounted = typeof(InvokedEventArgs);
    public static readonly Type OnMounting = typeof(InvokingEventArgs);

    public static readonly Type Priority = typeof(int);
}
