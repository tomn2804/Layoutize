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
    }

    internal override sealed DirectoryView CreateView(IBuildContext context)
    {
        Debug.Assert(!context.Element.IsDisposed);
        Debug.Assert(context.Element.Parent == null);
        string fullName = Path.Combine((string)Attributes["Path"], (string)Attributes["Name"]);
        Debug.Assert(Path.IsPathFullyQualified(fullName));
        return new(new(fullName));
    }
}
