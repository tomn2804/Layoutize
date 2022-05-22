using System.IO;
using System.Diagnostics;

namespace Layoutize.Views;

internal abstract class FileSystemView : View
{
    private protected readonly FileSystemInfo FileSystemInfo;

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
            Debug.Assert(Utils.Path.IsValid(fullName));
            return fullName;
        }
    }

    internal override string Name
    {
        get
        {
            string name = FileSystemInfo.Name;
            Debug.Assert(Utils.Name.IsValid(name));
            return name;
        }
    }

    internal override abstract void Create();

    internal override void Delete()
    {
        Debug.Assert(Exists);
        FileSystemInfo.Delete();
        Debug.Assert(!Exists);
    }
}
