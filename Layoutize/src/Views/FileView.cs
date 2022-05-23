using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal class FileView : View
{
    public FileView(FileInfo fileInfo)
        : base(fileInfo)
    {
    }

    private FileInfo FileInfo => (FileInfo)FileSystemInfo;

    public override void Create()
    {
        Debug.Assert(!Exists);
        FileInfo.Create().Dispose();
        Debug.Assert(Exists);
    }
}
