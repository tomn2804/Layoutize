using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal class FileView : FileSystemView
{
	public FileView(FileInfo fileInfo)
	{
		FileSystemInfo = fileInfo;
	}

	public override void Create()
	{
		Debug.Assert(!Exists);
		FileSystemInfo.Create().Dispose();
		Debug.Assert(Exists);
	}

	protected override FileInfo FileSystemInfo { get; }
}
