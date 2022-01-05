using System.Collections.Generic;

namespace Templata;

public abstract partial class Model
{
    public class Comparer : IComparer<Model>
    {
        public int Compare(Model? x, Model? y)
        {
            return x?.CompareTo(y) ?? y?.CompareTo(x) ?? 0;
        }
    }
}
