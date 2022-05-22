using Layoutize.Elements;
using Layoutize.Contexts;
using Layoutize.Views;
using System.Diagnostics;

namespace Layoutize;

public class FileLayout : ViewLayout
{
    internal override FileElement CreateElement()
    {
        return new(this);
    }

    internal override FileView CreateView(IBuildContext context)
    {
        string fullName = System.IO.Path.Combine(Path.Of(context), Name);
        Debug.Assert(FullName.IsValid(fullName));
        return new(new(fullName));
    }
}
