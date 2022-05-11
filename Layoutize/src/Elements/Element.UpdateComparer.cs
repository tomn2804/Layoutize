using Layoutize.Attributes;
using System.Collections.Generic;

namespace Layoutize.Elements;

internal abstract partial class Element
{
    private protected sealed partial class UpdateComparer : IEqualityComparer<Element>
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
            return Name.RequireOf(obj).GetHashCode();
        }
    }
}
