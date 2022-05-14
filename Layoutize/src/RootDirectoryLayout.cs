using Layoutize.Attributes;
using System.Collections;

namespace Layoutize;

internal sealed class RootDirectoryLayout : DirectoryLayout
{
    internal RootDirectoryLayout(IDictionary attributes)
        : base(attributes)
    {
        Path.RequireOf(Attributes);
    }
}
