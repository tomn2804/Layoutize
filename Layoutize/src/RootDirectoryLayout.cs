using Layoutize.Elements;
using Layoutize.Utils;
using Layoutize.Views;
using System.Collections;
using System.Diagnostics;

namespace Layoutize;

internal sealed class RootDirectoryLayout : DirectoryLayout
{
    internal RootDirectoryLayout(IDictionary attributes)
        : base(attributes)
    {
        Debug.Assert(Attributes.TryGetValue("Path", out object? pathObject));
        Debug.Assert(pathObject?.ToString() != null);
        Debug.Assert(System.IO.Path.IsPathFullyQualified(pathObject?.ToString()!));
    }

    internal override sealed DirectoryView CreateView(IBuildContext context)
    {
        string fullName = System.IO.Path.GetFullPath(System.IO.Path.Combine((string)Attributes["Path"], Name.Of(context)));
        Debug.Assert(System.IO.Path.IsPathFullyQualified(fullName));
        return new(new(fullName));
    }
}
