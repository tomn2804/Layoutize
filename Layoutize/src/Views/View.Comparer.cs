using System.Collections.Generic;

namespace Templatize.Views;

public abstract partial class View
{
    public class Comparer : IComparer<View>
    {
        public int Compare(View? x, View? y)
        {
            return x?.CompareTo(y) ?? y?.CompareTo(x) ?? 0;
        }
    }
}
