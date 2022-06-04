using System;
using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal sealed class RootDirectoryView : IView
{
	public RootDirectoryView(DirectoryInfo directoryInfo)
	{
		Debug.Assert(directoryInfo.Exists);
		_directoryInfo = directoryInfo;
	}

	public void Create()
	{
		throw new InvalidOperationException();
	}

	public void Delete()
	{
		throw new InvalidOperationException();
	}

	public bool Exists => _directoryInfo.Exists;

	public string FullName
	{
		get
		{
			var fullName = _directoryInfo.FullName;
			Debug.Assert(Contexts.FullName.IsValid(fullName));
			return fullName;
		}
	}

	public string Name => throw new InvalidOperationException();

	private readonly DirectoryInfo _directoryInfo;
}
