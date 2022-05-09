using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal sealed class DirectoryView : View
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
    }

    internal override sealed void Delete()
    {
        Debug.Assert(Exists);
        DirectoryInfo.Delete(true);
    }
}
