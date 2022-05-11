using Layoutize.Attributes;
using Layoutize.Elements;
using Layoutize.Views;
using System.Collections;
using System.Diagnostics;

namespace Layoutize;

public class FileLayout : ViewLayout
{
    public FileLayout(IEnumerable attributes)
        : base(attributes)
    {
    }

    internal override FileElement CreateElement()
    {
        return new(this);
    }

    internal override FileView CreateView(IBuildContext context)
    {
        string fullName = System.IO.Path.Combine(Path.RequireOf(context), Name.RequireOf(context));
        Debug.Assert(System.IO.Path.IsPathFullyQualified(fullName));
        return new(new(fullName));
    }
}
