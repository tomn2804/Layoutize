using System.IO;
using System.Diagnostics;

namespace Layoutize.Views;

internal abstract class FileSystemView : View
{
    private protected FileSystemView(FileSystemInfo fileSystemInfo)
    {
        FileSystemInfo = fileSystemInfo;
    }

    internal override bool Exists => FileSystemInfo.Exists;

    internal override string FullName
    {
        get
        {
            string fullName = FileSystemInfo.FullName;
            Debug.Assert(Elements.Path.IsValid(fullName));
            return fullName;
        }
    }

    internal override string Name
    {
        get
        {
            string name = FileSystemInfo.Name;
            Debug.Assert(Elements.Name.IsValid(name));
            return name;
        }
    }

    private protected FileSystemInfo FileSystemInfo { get; }

    internal override sealed void Delete()
    {
        Debug.Assert(Exists);
        FileSystemInfo.Delete();
        Debug.Assert(!Exists);
    }
}
