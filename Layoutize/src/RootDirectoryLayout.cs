using Layoutize.Elements;
using Layoutize.Views;
using System.Collections;
using System.Diagnostics;
using System.IO;

namespace Layoutize;

internal sealed class RootDirectoryLayout : DirectoryLayout
{
    internal RootDirectoryLayout(IDictionary attributes)
        : base(attributes)
    {
        Debug.Assert(Attributes.ContainsKey("Path"));
        Debug.Assert(Path.IsPathFullyQualified(FullName));
    }

    private string FullName => Path.GetFullPath(Path.Combine((string)Attributes["Path"], (string)Attributes["Name"]));

    internal override sealed DirectoryView CreateView(IBuildContext context)
    {
        Debug.Assert(!context.Element.IsDisposed);
        Debug.Assert(context.Element.Parent == null);
        Debug.Assert(Path.IsPathFullyQualified(FullName));
        return new(new(FullName));
    }
}
