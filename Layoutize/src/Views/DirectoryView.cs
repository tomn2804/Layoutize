using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal sealed class DirectoryView : View
{
    internal DirectoryView(DirectoryInfo directoryInfo)
        : base(directoryInfo)
    {
    }

    internal override sealed string Name
    {
        get => FileSystemInfo.Name;
        set
        {
            Debug.Assert(Exists);
            Debug.Assert(Parent != null);
            DirectoryInfo.MoveTo(Path.Combine(Parent, value));
        }
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
