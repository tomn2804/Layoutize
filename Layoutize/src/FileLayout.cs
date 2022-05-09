using Layoutize.Elements;
using Layoutize.Utils;
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
        Debug.Assert(context.Element.Parent != null);
        string fullName = System.IO.Path.Combine(Path.Of(context), (string)Attributes["Name"]);
        return new(new(fullName));
    }
}
