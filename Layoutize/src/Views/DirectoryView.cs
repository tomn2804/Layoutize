using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal sealed class DirectoryView : FileSystemView
{
    internal DirectoryView(DirectoryInfo directoryInfo)
        : base(directoryInfo)
    {
    }

    private DirectoryInfo DirectoryInfo => (DirectoryInfo)FileSystemInfo;

    internal override sealed void Create()
    {
        Debug.Assert(!Exists);
        DirectoryInfo.Create();
        Debug.Assert(Exists);
    }
}
