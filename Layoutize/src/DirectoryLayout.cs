using Layoutize.Elements;
using Layoutize.Contexts;
using Layoutize.Views;
using System.Diagnostics;

namespace Layoutize;

public class DirectoryLayout : ViewGroupLayout
{
    internal override DirectoryElement CreateElement()
    {
        return new(this);
    }

    internal override DirectoryView CreateView(IBuildContext context)
    {
        string fullName = System.IO.Path.Combine(Path.Of(context), Name);
        Debug.Assert(FullName.IsValid(fullName));
        return new(new(fullName));
    }
}
