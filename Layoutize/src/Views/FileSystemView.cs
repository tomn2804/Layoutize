using Layoutize.Annotations;
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

	[FullName]
	public string FullName
	{
		get
		{
			var fullName = FileSystemInfo.FullName;
			Debug.Assert(this.IsMemberValid(nameof(FullName), fullName));
			return fullName;
		}
	}

	[Name]
	public virtual string Name
	{
		get
		{
			var name = FileSystemInfo.Name;
			Debug.Assert(this.IsMemberValid(nameof(Name), name));
			return name;
		}
	}

	protected FileSystemView(FileSystemInfo fileSystemInfo)
	{
		FileSystemInfo = fileSystemInfo;
	}

	protected readonly FileSystemInfo FileSystemInfo;
}
