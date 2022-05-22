using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal sealed class DirectoryView : FileSystemView
{
    internal DirectoryView(DirectoryInfo directoryInfo)
        : base(directoryInfo)
    {
    }

    private new DirectoryInfo FileSystemInfo => (DirectoryInfo)base.FileSystemInfo;

    internal sealed override void Create()
    {
        Debug.Assert(!Exists);
        FileSystemInfo.Create();
        Debug.Assert(Exists);
    }
}
