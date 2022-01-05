using System;
using System.IO;

namespace Schemata;

public partial class FileModel : Model
{
    public override FileTree Tree { get; }

    public virtual void Create()
    {
        File.Create(FullName).Dispose();
    }

    protected FileModel(Blueprint blueprint)
            : base(blueprint)
    {
        if (Name.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
        {
            throw new ArgumentException($"Details value property '{Template.DetailOption.Name}' cannot contain invalid system characters.", nameof(blueprint));
        }
        Tree = new(this);
    }
}
