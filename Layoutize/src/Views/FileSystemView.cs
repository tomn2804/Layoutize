using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal abstract class FileSystemView : IView
{
	public abstract void Create();

	public virtual void Delete()
	{
		Debug.Assert(Exists);
		FileSystemInfo.Delete();
		Debug.Assert(!Exists);
	}

	public bool Exists => FileSystemInfo.Exists;

	public string FullName
	{
		get
		{
			var fullName = FileSystemInfo.FullName;
			Debug.Assert(Contexts.FullName.IsValid(fullName));
			return fullName;
		}
	}

	public virtual string Name
	{
		get
		{
			var name = FileSystemInfo.Name;
			Debug.Assert(Contexts.Name.IsValid(name));
			return name;
		}
	}

	protected FileSystemView(FileSystemInfo fileSystemInfo)
	{
		FileSystemInfo = fileSystemInfo;
	}

	protected readonly FileSystemInfo FileSystemInfo;
}
