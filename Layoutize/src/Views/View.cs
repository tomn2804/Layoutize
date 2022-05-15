using System.IO;
using System.Diagnostics;

namespace Layoutize.Views;

internal abstract class View
{
    private protected View(FileSystemInfo fileSystemInfo)
    {
        FileSystemInfo = fileSystemInfo;
        Debug.Assert(Attributes.Name.IsValid(Name));
        Debug.Assert(Attributes.Path.IsValid(FullName));
    }

    internal bool Exists => FileSystemInfo.Exists;

    internal string FullName => FileSystemInfo.FullName;

    internal string Name => FileSystemInfo.Name;

    private protected FileSystemInfo FileSystemInfo { get; }

    internal abstract void Create();

    internal abstract void Delete();
}
