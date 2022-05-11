﻿using System.Diagnostics;
using System.IO;

namespace Layoutize.Views;

internal abstract class View
{
    private protected View(FileSystemInfo fileSystemInfo)
    {
        Debug.Assert(!Attributes.Name.IsValid(fileSystemInfo.Name));
        Debug.Assert(Attributes.Path.IsValid(fileSystemInfo.FullName));
        FileSystemInfo = fileSystemInfo;
    }

    internal bool Exists => FileSystemInfo.Exists;

    internal string FullName => FileSystemInfo.FullName;

    internal string Name => FileSystemInfo.Name;

    internal string? Parent => Path.GetDirectoryName(FullName);

    private protected FileSystemInfo FileSystemInfo { get; }

    internal abstract void Create();

    internal abstract void Delete();
}
