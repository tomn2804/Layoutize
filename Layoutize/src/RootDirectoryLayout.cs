using Layoutize.Elements;
using Layoutize.Views;
using System.Collections;

namespace Layoutize;

internal sealed class RootDirectoryLayout : DirectoryLayout
{
    internal RootDirectoryLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    internal override sealed DirectoryView CreateView(IBuildContext context)
    {
        string fullName = (string)Attributes["FullName"];
        return new(new(fullName));
    }
}
