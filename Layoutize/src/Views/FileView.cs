using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal class FileView : FileSystemView
{
	public FileView(FileInfo fileInfo)
		: base(fileInfo)
	{
	}

	public override void Create()
	{
		Debug.Assert(!Exists);
		FileInfo.Create().Dispose();
		Debug.Assert(Exists);
	}

	private FileInfo FileInfo => (FileInfo)FileSystemInfo;
}
