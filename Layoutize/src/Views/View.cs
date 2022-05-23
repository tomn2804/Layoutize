using System.IO;
using System.Diagnostics;

namespace Layoutize.Views;

internal abstract class View
{
    protected readonly FileSystemInfo FileSystemInfo;

    protected View(FileSystemInfo fileSystemInfo)
    {
        FileSystemInfo = fileSystemInfo;
    }

    public virtual bool Exists => FileSystemInfo.Exists;

    public virtual string FullName
    {
        get
        {
            string fullName = FileSystemInfo.FullName;
            Debug.Assert(Contexts.FullName.IsValid(fullName));
            return fullName;
        }
    }

    public virtual string Name
    {
        get
        {
            string name = FileSystemInfo.Name;
            Debug.Assert(Contexts.Name.IsValid(name));
            return name;
        }
    }

    public abstract void Create();

    public virtual void Delete()
    {
        Debug.Assert(Exists);
        FileSystemInfo.Delete();
        Debug.Assert(!Exists);
    }
}
