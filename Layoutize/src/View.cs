using System.IO;

namespace Layoutize;

internal abstract class View
{
    private protected View(FileSystemInfo fileSystemInfo)
    {
        FileSystemInfo = fileSystemInfo;
    }

    internal bool Exists => FileSystemInfo.Exists;

    internal string FullName => FileSystemInfo.FullName;

    internal string? Parent => System.IO.Path.GetDirectoryName(FullName);

    internal abstract string Name { get; set; }

    private protected FileSystemInfo FileSystemInfo { get; }

    internal abstract void Create();

    internal abstract void Delete();
}
