using Layoutize.Elements;
using Layoutize.Views;
using System.Collections;

namespace Layoutize;

public abstract class ViewLayout : Layout
{
    private protected ViewLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    internal abstract override ViewElement CreateElement();

    internal abstract View CreateView(IBuildContext context);
}
