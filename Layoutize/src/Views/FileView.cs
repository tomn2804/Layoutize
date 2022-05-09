using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal sealed class FileView : View
{
    internal FileView(FileInfo fileInfo)
        : base(fileInfo)
    {
    }

    internal override sealed void Create()
    {
        Debug.Assert(!Exists);
        Debug.Assert(Parent != null);
        FileInfo.Create().Dispose();
    }

    internal override sealed void Delete()
    {
        Debug.Assert(Exists);
        Debug.Assert(Parent != null);
        FileInfo.Delete();
    }

    private FileInfo FileInfo => (FileInfo)FileSystemInfo;
}
