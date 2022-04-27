using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal sealed class FileView : View
{
    internal FileView(FileInfo fileSystemInfo)
        : base(fileSystemInfo)
    {
    }

    internal override sealed string Name
    {
        get => FileSystemInfo.Name;
        set
        {
            Debug.Assert(Exists);
            Debug.Assert(Parent != null);
            FileSystemInfo.MoveTo(Path.Combine(Parent, value));
        }
    }

    internal override sealed void Create()
    {
        Debug.Assert(!Exists);
        Debug.Assert(Parent != null);
        FileSystemInfo.Create();
    }

    internal override sealed void Delete()
    {
        Debug.Assert(Exists);
        Debug.Assert(Parent != null);
        FileSystemInfo.Delete();
    }

    private new FileInfo FileSystemInfo => FileSystemInfo;
}
