using Layoutize.Attributes;
using Layoutize.Elements;
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
        string fullName = System.IO.Path.Combine(Path.RequireOf(context), Name.RequireOf(context));
        Debug.Assert(Path.TryValidate(fullName));
        return new(new(fullName));
    }
}
