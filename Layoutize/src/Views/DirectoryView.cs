using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal sealed class DirectoryView : FileSystemView
{
	public DirectoryView(DirectoryInfo directoryInfo)
		: base(directoryInfo)
	{
	}

	public override void Create()
	{
		Debug.Assert(!Exists);
		DirectoryInfo.Create();
		Debug.Assert(Exists);
	}

	private DirectoryInfo DirectoryInfo => (DirectoryInfo)FileSystemInfo;
}
