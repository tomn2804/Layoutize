using Layoutize.Attributes;
using System.Collections;
using System.Diagnostics;

namespace Layoutize;

internal sealed class RootDirectoryLayout : DirectoryLayout
{
    internal RootDirectoryLayout(IEnumerable attributes)
        : base(attributes)
    {
        Debug.Assert(Path.RequireOf(this) != null);
    }
}
