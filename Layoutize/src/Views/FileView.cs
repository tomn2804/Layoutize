using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal sealed class FileView : FileSystemView
{
    internal FileView(FileInfo fileInfo)
        : base(fileInfo)
    {
    }

    private new FileInfo FileSystemInfo => (FileInfo)base.FileSystemInfo;

    internal sealed override void Create()
    {
        Debug.Assert(!Exists);
        FileSystemInfo.Create().Dispose();
        Debug.Assert(Exists);
    }
}
