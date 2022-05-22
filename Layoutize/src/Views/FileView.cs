using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal class FileView : View
{
    public FileView(FileInfo file)
        : base(file)
    {
    }

    protected new FileInfo FileSystem => (FileInfo)base.FileSystem;

    public override void Create()
    {
        Debug.Assert(!Exists);
        FileSystem.Create().Dispose();
        Debug.Assert(Exists);
    }
}
