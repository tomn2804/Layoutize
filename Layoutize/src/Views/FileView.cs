using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal sealed class FileView : View
{
    internal FileView(FileInfo fileInfo)
        : base(fileInfo)
    {
        Debug.Assert(Parent != null);
    }

    private FileInfo FileInfo => (FileInfo)FileSystemInfo;

    internal override sealed void Create()
    {
        Debug.Assert(Parent != null);
        Debug.Assert(!Exists);
        FileInfo.Create().Dispose();
    }

    internal override sealed void Delete()
    {
        Debug.Assert(Parent != null);
        Debug.Assert(Exists);
        FileInfo.Delete();
    }
}
