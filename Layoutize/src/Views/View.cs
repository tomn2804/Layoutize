using System.IO;

namespace Layoutize.Views;

internal abstract class View
{
    internal bool Exists => FileSystemInfo.Exists;

    internal string FullName => FileSystemInfo.FullName;

    internal string Name => FileSystemInfo.Name;

    internal string? Parent => Path.GetDirectoryName(FullName);

    internal abstract void Create();

    internal abstract void Delete();

    private protected View(FileSystemInfo fileSystemInfo)
    {
        FileSystemInfo = fileSystemInfo;
    }

    private protected FileSystemInfo FileSystemInfo { get; }
}
