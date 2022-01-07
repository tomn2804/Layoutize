using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Templata;

public sealed partial class Context
{
    internal sealed class Builder
    {
        internal Builder()
        {
            Path = Directory.GetCurrentDirectory();
            Templates = new();
        }

        internal Builder(Context context)
        {
            Path = context.Path;
            Templates = context.Templates.ToList();
            Debug.Assert(context.Details == Details);
            Debug.Assert(context.ViewType == ViewType);
        }

        internal IReadOnlyDictionary<object, object> Details => Templates.FirstOrDefault()?.Details ?? ImmutableDictionary.Create<object, object>();

        internal Type ViewType => Templates.LastOrDefault()?.ViewType ?? typeof(View);

        internal string Path { get; set; }

        internal List<Template> Templates { get; }

        internal Context ToBlueprint()
        {
            return new() { Details = Details, ViewType = ViewType, Path = Path, Templates = Templates.ToImmutableList() };
        }
    }
}
