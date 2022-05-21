using Layoutize.Elements;
using Layoutize.Views;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace Layoutize;

public class PathTypeConverter : TypeConverter
{
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        string? path = value.ToString();
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new InvalidProgramException($"Attribute value 'Path' is null or contains only white spaces.");
        }
        if (path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
        {
            throw new InvalidProgramException($"Attribute value 'Path' contains invalid characters.");
        }
        if (!System.IO.Path.IsPathFullyQualified(path))
        {
            throw new InvalidProgramException($"Attribute value 'Path' is not an absolute path.");
        }
        return path;
    }
}

internal sealed class RootDirectoryLayout : DirectoryLayout
{
    internal RootDirectoryLayout(IDictionary attributes)
        : base(attributes)
    {
        Debug.Assert(Path != null);
    }

    [TypeConverter(typeof(PathTypeConverter))]
    internal string Path { get; }

    internal override DirectoryView CreateView(IBuildContext context)
    {
        string fullName = System.IO.Path.Combine(Path, Name);
        Debug.Assert(Views.Path.IsValid(fullName));
        return new(new(fullName));
    }
}
