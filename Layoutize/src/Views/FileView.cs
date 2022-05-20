using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal sealed class FileView : FileSystemView
{
    internal FileView(FileInfo fileInfo)
        : base(fileInfo)
    {
    }

    private FileInfo FileInfo => (FileInfo)FileSystemInfo;

    internal override sealed void Create()
    {
        Debug.Assert(!Exists);
        FileInfo.Create().Dispose();
        Debug.Assert(Exists);
    }
}
