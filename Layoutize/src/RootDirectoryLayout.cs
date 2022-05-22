using Layoutize.Elements;
using Layoutize.Contexts;
using Layoutize.Views;
using System.Diagnostics;

namespace Layoutize;

internal sealed class RootDirectoryLayout : DirectoryLayout
{
    internal string Path { get; init; } = string.Empty;

    internal override DirectoryView CreateView(IBuildContext context)
    {
        string fullName = System.IO.Path.Combine(Path, Name);
        Debug.Assert(FullName.IsValid(fullName));
        return new(new(fullName));
    }
}
