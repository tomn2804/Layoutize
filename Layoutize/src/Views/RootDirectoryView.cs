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
		throw new InvalidOperationException();
	}

	public override void Delete()
	{
		throw new InvalidOperationException();
	}

	public override string Name => throw new InvalidOperationException();
}
