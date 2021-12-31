using System;
using System.Collections.Generic;
using System.IO;

namespace Schemata;

public class FileModel : Model
{
    public override FileTree Tree { get; }

    protected FileModel(Blueprint blueprint)
        : base(blueprint)
    {
        Tree = new(this);
    }

    public virtual void Create()
    {
        File.Create(FullName).Dispose();
    }
}
