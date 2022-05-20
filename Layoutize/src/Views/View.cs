using System.IO;
using System.Diagnostics;

namespace Layoutize.Views;

internal abstract class View
{
    private protected View(FileSystemInfo fileSystemInfo)
    {
        FileSystemInfo = fileSystemInfo;
    }

    internal bool Exists => FileSystemInfo.Exists;

    internal string FullName
    {
        get
        {
            string fullName = FileSystemInfo.FullName;
            Debug.Assert(Attributes.Path.IsValid(fullName));
            return fullName;
        }
    }

    internal string Name
    {
        get
        {
            string name = FileSystemInfo.Name;
            Debug.Assert(Attributes.Name.IsValid(name));
            return name;
        }
    }

    private protected FileSystemInfo FileSystemInfo { get; }

    internal abstract void Create();

    internal abstract void Delete();
}
