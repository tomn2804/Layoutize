using Layoutize.Elements;
using Layoutize.Contexts;
using Layoutize.Views;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace Layoutize;

public class DirectoryLayout : ViewGroupLayout
{
    public new object Children
    {
        get => base.Children;
        init
        {
            base.Children = value switch
            {
                IEnumerable<object> children => children.Cast<Layout>(),
                _ => new[] { (Layout)value },
            };
        }
    }

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
