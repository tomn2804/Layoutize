using System;
using System.Diagnostics;
using System.IO;

namespace Layoutize;

public abstract class FileSystemView : View
{
    protected FileSystemView(FileSystemInfo fileSystemInfo)
        : base(fileSystemInfo)
    {
    }

    public abstract void Create();

    public abstract void Delete();
}

public class DirectoryView : FileSystemView
{
    public DirectoryView(DirectoryInfo fileSystemInfo)
        : base(fileSystemInfo)
    {
    }

    public new string Name
    {
        get => base.Name;
        set
        {
            Debug.Assert(Exists);
            if (Parent == null) throw new InvalidOperationException();
            FileSystemInfo.MoveTo(Path.Combine(Parent, value));
        }
    }

    protected new DirectoryInfo FileSystemInfo => FileSystemInfo;

    public override void Create()
    {
        Debug.Assert(!Exists);
        FileSystemInfo.Create();
    }

    public override void Delete()
    {
        Debug.Assert(Exists);
        FileSystemInfo.Delete();
    }
}

public class FileView : FileSystemView
{
    public FileView(FileInfo fileSystemInfo)
        : base(fileSystemInfo)
    {
    }

    public new string Name
    {
        get => base.Name;
        set
        {
            Debug.Assert(Exists);
            if (Parent == null) throw new InvalidOperationException();
            FileSystemInfo.MoveTo(Path.Combine(Parent, value));
        }
    }

    protected new FileInfo FileSystemInfo => FileSystemInfo;

    public override void Create()
    {
        Debug.Assert(!Exists);
        FileSystemInfo.Create();
    }

    public override void Delete()
    {
        Debug.Assert(Exists);
        FileSystemInfo.Delete();
    }
}
