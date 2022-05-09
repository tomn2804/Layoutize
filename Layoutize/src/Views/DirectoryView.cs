using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal sealed class DirectoryView : View
{
    internal DirectoryView(DirectoryInfo directoryInfo)
        : base(directoryInfo)
    {
    }

    internal override sealed void Create()
    {
        Debug.Assert(!Exists);
        Debug.Assert(Parent != null);
        DirectoryInfo.Create();
    }

    internal override sealed void Delete()
    {
        Debug.Assert(Exists);
        Debug.Assert(Parent != null);
        DirectoryInfo.Delete(true);
    }

    private DirectoryInfo DirectoryInfo => (DirectoryInfo)FileSystemInfo;
}
