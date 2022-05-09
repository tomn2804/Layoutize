using Layoutize.Elements;
using Layoutize.Utils;
using Layoutize.Views;
using System.Collections;
using System.Diagnostics;

namespace Layoutize;

public class DirectoryLayout : ViewGroupLayout
{
    public DirectoryLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    internal override DirectoryElement CreateElement()
    {
        return new(this);
    }

    internal override DirectoryView CreateView(IBuildContext context)
    {
        Debug.Assert(context.Element.Parent != null);
        string fullName = System.IO.Path.Combine(Path.Of(context), (string)Attributes["Name"]);
        return new(new(fullName));
    }
}
