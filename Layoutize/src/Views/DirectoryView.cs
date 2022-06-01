using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal class DirectoryView : FileSystemView
{
	public DirectoryView(DirectoryInfo directoryInfo)
	{
		FileSystemInfo = directoryInfo;
	}

	public override void Create()
	{
		Debug.Assert(!Exists);
		FileSystemInfo.Create();
		Debug.Assert(Exists);
	}

	protected override DirectoryInfo FileSystemInfo { get; }
}
