using System.IO;

namespace Layoutize;

public abstract class View
{
    protected View(FileSystemInfo fileSystemInfo)
    {
        FileSystemInfo = fileSystemInfo;
    }

    public bool Exists => FileSystemInfo.Exists;

    public string FullName => FileSystemInfo.FullName;

    public string? Parent => Path.GetDirectoryName(FullName);

    public string Name => FileSystemInfo.Name;

    protected FileSystemInfo FileSystemInfo { get; }
}
