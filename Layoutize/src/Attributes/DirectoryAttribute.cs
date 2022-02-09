using System;
using System.Collections.Generic;

namespace Templatize.Attributes;

public class DirectoryAttribute : FileSystemBindingAttribute
{
    public static readonly Type Children = typeof(IEnumerable<object>);
}
