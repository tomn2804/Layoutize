using Layoutize.Attributes;
using Layoutize.Elements;
using Layoutize.Views;
using System.Collections;
using System.Diagnostics;

namespace Layoutize;

internal sealed class RootDirectoryLayout : DirectoryLayout
{
    internal RootDirectoryLayout(IDictionary attributes)
        : base(attributes)
    {
        Path.RequireOf(Attributes);
    }

    internal override DirectoryView CreateView(IBuildContext context)
    {
        string fullName = System.IO.Path.Combine(Path.RequireOf(Attributes), Name.RequireOf(Attributes));
        Debug.Assert(Path.IsValid(fullName));
        return new(new(fullName));
    }
}
