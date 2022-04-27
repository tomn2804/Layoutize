using Layoutize.Elements;
using Layoutize.Utils;
using Layoutize.Views;
using System.Collections;

namespace Layoutize;

public sealed class DirectoryLayout : ViewGroupLayout
{
    public DirectoryLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    internal override sealed DirectoryElement CreateElement()
    {
        return new(this);
    }

    internal override sealed DirectoryView CreateView(IBuildContext context)
    {
        string fullName = System.IO.Path.Combine(Path.Of(context), (string)Attributes["Name"]);
        return new(new(fullName));
    }
}
