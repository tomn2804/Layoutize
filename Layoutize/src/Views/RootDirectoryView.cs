using System;
using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal sealed class RootDirectoryView : DirectoryView
{
	public RootDirectoryView(DirectoryInfo directoryInfo)
		: base(directoryInfo)
	{
		Debug.Assert(directoryInfo.Exists);
	}

	public override void Create()
	{
		throw new NotSupportedException();
	}

	public override void Delete()
	{
		throw new NotSupportedException();
	}

	public override string Name => throw new NotSupportedException();
}
