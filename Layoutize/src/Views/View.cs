using System.IO;
using System.Diagnostics;

namespace Layoutize.Views;

internal abstract class View
{
    private protected View(FileSystemInfo fileSystemInfo)
    {
        FileSystemInfo = fileSystemInfo;
        Debug.Assert(Attributes.Name.TryValidate(Name));
        Debug.Assert(Attributes.Name.TryValidate(FullName));
    }

    internal bool Exists => FileSystemInfo.Exists;

    internal string FullName => FileSystemInfo.FullName;

    internal string Name => FileSystemInfo.Name;

    internal string? Parent => Path.GetDirectoryName(FullName);

    private protected FileSystemInfo FileSystemInfo { get; }

    internal abstract void Create();

    internal abstract void Delete();
}
