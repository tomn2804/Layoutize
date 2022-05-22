using System.IO;
using System.Diagnostics;

namespace Layoutize.Views;

internal abstract class View
{
    protected readonly FileSystemInfo FileSystem;

    protected View(FileSystemInfo fileSystem)
    {
        FileSystem = fileSystem;
    }

    public virtual bool Exists => FileSystem.Exists;

    public virtual string FullName
    {
        get
        {
            string fullName = FileSystem.FullName;
            Debug.Assert(Contexts.FullName.IsValid(fullName));
            return fullName;
        }
    }

    public virtual string Name
    {
        get
        {
            string name = FileSystem.Name;
            Debug.Assert(Contexts.Name.IsValid(name));
            return name;
        }
    }

    public abstract void Create();

    public virtual void Delete()
    {
        Debug.Assert(Exists);
        FileSystem.Delete();
        Debug.Assert(!Exists);
    }
}
