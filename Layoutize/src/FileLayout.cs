using Layoutize.Elements;
using Layoutize.Views;
using System.Collections;
using System.Diagnostics;

namespace Layoutize;

public class FileLayout : ViewLayout
{
    public FileLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    internal override FileElement CreateElement()
    {
        return new(this);
    }

    internal override FileView CreateView(IBuildContext context)
    {
        string fullName = System.IO.Path.Combine(Path.RequireOf(context), Name);
        Debug.Assert(Path.IsValid(fullName));
        return new(new(fullName));
    }
}
