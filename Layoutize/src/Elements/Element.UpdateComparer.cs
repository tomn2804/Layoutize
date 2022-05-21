using System.Collections.Generic;

namespace Layoutize.Elements;

internal abstract partial class Element
{
    private protected sealed class UpdateComparer : IEqualityComparer<Element>
    {
        public bool Equals(Element? x, Element? y)
        {
            if (x == null || y == null)
            {
                return x == y;
            }
            return x.Layout.GetType().Equals(y.Layout.GetType()) && (x.CompareTo(y) == 0);
        }

        public int GetHashCode(Element obj)
        {
            return obj.View.Name.GetHashCode();
        }
    }
}
