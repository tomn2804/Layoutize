using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal class DirectoryView : View
{
    public DirectoryView(DirectoryInfo directory)
        : base(directory)
    {
    }

    protected new DirectoryInfo FileSystem => (DirectoryInfo)base.FileSystem;

    public override void Create()
    {
        Debug.Assert(!Exists);
        FileSystem.Create();
        Debug.Assert(Exists);
    }
}
