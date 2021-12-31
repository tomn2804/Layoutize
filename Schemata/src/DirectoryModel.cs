using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Schemata;

public class DirectoryModel : Model
{
    public List<Model> Children { get; } = new();

    public override DirectoryLevelOrderTree Tree { get; }

    public DirectoryModel(Blueprint blueprint)
        : base(blueprint)
    {
        Tree = new(this);
    }

    protected virtual void Create()
    {
        Directory.CreateDirectory($"{Path}\\{Name}");
    }
}
