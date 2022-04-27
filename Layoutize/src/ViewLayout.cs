using Layoutize.Elements;
using Layoutize.Views;
using System.Collections;

namespace Layoutize;

public abstract class ViewLayout : Layout
{
    internal abstract override ViewElement CreateElement();

    internal abstract View CreateView(IBuildContext context);

    private protected ViewLayout(IDictionary attributes)
        : base(attributes)
    {
    }
}
