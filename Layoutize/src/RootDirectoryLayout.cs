using Layoutize.Elements;
using Layoutize.Views;
using System.Collections;
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
        string fullName = Path.Combine((string)Attributes["Path"], (string)Attributes["Name"]);
        return new(new(fullName));
    }
}
