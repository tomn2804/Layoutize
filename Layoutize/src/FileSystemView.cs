using System.Diagnostics;
using System.IO;

namespace Layoutize;

internal class FileView : View
{
    internal FileView(FileInfo fileSystemInfo)
        : base(fileSystemInfo)
    {
    }

    internal override string Name
    {
        get => FileSystemInfo.Name;
        set
        {
            Debug.Assert(Exists);
            Debug.Assert(Parent != null);
            FileSystemInfo.MoveTo(System.IO.Path.Combine(Parent, value));
        }
    }

    private new FileInfo FileSystemInfo => FileSystemInfo;

    internal override void Create()
    {
        Debug.Assert(!Exists);
        Debug.Assert(Parent != null);
        FileSystemInfo.Create();
    }

    internal override void Delete()
    {
        Debug.Assert(Exists);
        Debug.Assert(Parent != null);
        FileSystemInfo.Delete();
    }
}

internal class DirectoryView : View
{
    internal DirectoryView(DirectoryInfo fileSystemInfo)
        : base(fileSystemInfo)
    {
    }

    internal override string Name
    {
        get => FileSystemInfo.Name;
        set
        {
            Debug.Assert(Exists);
            Debug.Assert(Parent != null);
            FileSystemInfo.MoveTo(System.IO.Path.Combine(Parent, value));
        }
    }

    private new DirectoryInfo FileSystemInfo => FileSystemInfo;

    internal override void Create()
    {
        Debug.Assert(!Exists);
        Debug.Assert(Parent != null);
        FileSystemInfo.Create();
    }

    internal override void Delete()
    {
        Debug.Assert(Exists);
        Debug.Assert(Parent != null);
        FileSystemInfo.Delete();
    }
}
