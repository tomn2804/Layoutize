using Layoutize.Attributes;
using System.Collections;
using System.Diagnostics;

namespace Layoutize;

internal sealed class RootDirectoryLayout : DirectoryLayout
{
    internal RootDirectoryLayout(IDictionary attributes)
        : base(attributes)
    {
        Debug.Assert(Path.Of(this) != null);
    }
}
