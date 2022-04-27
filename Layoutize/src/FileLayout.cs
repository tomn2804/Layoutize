using Layoutize.Elements;
using Layoutize.Utils;
using Layoutize.Views;
using System.Collections;

namespace Layoutize;

public sealed class FileLayout : ViewLayout
{
    public FileLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    internal override sealed FileElement CreateElement()
    {
        return new(this);
    }

    internal override sealed FileView CreateView(IBuildContext context)
    {
        string fullName = System.IO.Path.Combine(Path.Of(context), (string)Attributes["Name"]);
        return new(new(fullName));
    }
}
