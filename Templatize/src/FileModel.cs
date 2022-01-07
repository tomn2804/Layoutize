using System;
using System.IO;

namespace Templata;

public partial class FileView : View
{
    public override bool Exists => File.Exists(FullName);

    public override FileTree Tree { get; }

    public virtual void Create()
    {
        File.Create(FullName).Dispose();
    }

    protected FileView(Context context)
        : base(context)
    {
        if (Name.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
        {
            throw new ArgumentException($"Details value property '{Template.DetailOption.Name}' cannot contain invalid system characters.", nameof(context));
        }
        Tree = new(this);
    }
}
