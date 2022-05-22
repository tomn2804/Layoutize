using Layoutize.Elements;
using Layoutize.Contexts;
using Layoutize.Views;
using System.Diagnostics;

namespace Layoutize;

internal class RootDirectoryLayout : DirectoryLayout
{
    private string _path = string.Empty;

    public string Path
    {
        get => _path;
        init
        {
            Debug.Assert(Contexts.Path.IsValid(value));
            _path = value;
        }
    }

    internal override DirectoryView CreateView(IBuildContext context)
    {
        string fullName = System.IO.Path.Combine(Path, Name);
        Debug.Assert(FullName.IsValid(fullName));
        return new(new(fullName));
    }
}
